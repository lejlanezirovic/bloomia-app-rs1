using Bloomia.Application.Modules.Therapists.Dashboard.Reports.Queries.List;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.TherapistRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Reports.Commands.Generate
{
    public class GenerateTherapistReportCommandHandler(IAppDbContext ctx, IAppCurrentUser currentUser, ITherapistReportPdfService pdfService, IFileStorageService fileStorageService) : IRequestHandler<GenerateTherapistReportCommand, TherapistReportListItemDto>
    {
        public async Task<TherapistReportListItemDto> Handle(GenerateTherapistReportCommand request, CancellationToken ct)
        {
            if (currentUser.UserId is null)
                throw new BloomiaBusinessRuleException("AUTH", "User is not authenticated.");

            if (request.DateFrom.Date > request.DateTo.Date)
                throw new BloomiaBusinessRuleException("REPORT_DATE_RANGE",
                    "Start date must be before end date.");

            var therapist = await ctx.Therapists.AsNoTracking()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == currentUser.UserId, ct);

            if (therapist == null)
                throw new BloomiaBusinessRuleException("THERAPIST", "Therapist was not found.");

            var client = await ctx.Clients.AsNoTracking()
                            .Include(x => x.User)
                            .FirstOrDefaultAsync(
                                x => x.Id == request.ClientId,
                                ct);

            if (client == null)
                throw new BloomiaBusinessRuleException("CLIENT", "Client was not found.");

            var dateFrom = request.DateFrom.Date;

            var dateToExclusive = request.DateTo.Date.AddDays(1);

            var now = DateTime.UtcNow;

            var appointments = await ctx.Appointments.AsNoTracking()
                .Where(x => x.TherapistAvailability.TherapistId == therapist.Id &&
                            x.ClientId == request.ClientId &&
                            x.ScheduledAtUtc >= dateFrom &&
                            x.ScheduledAtUtc < dateToExclusive)
                        .OrderBy(x => x.ScheduledAtUtc)
                        .ToListAsync(ct);

            var appointmentsCount = appointments.Count;

            var completedSessionsCount = appointments.Count(x => x.ScheduledAtUtc.AddHours(1) <= now);


            var reviewsQuery = ctx.Reviews.AsNoTracking()
                .Where(x => x.TherapistId == therapist.Id &&
                            x.ClientId == request.ClientId &&
                            x.CreatedAtUtc >= dateFrom && x.CreatedAtUtc < dateToExclusive);

            var averageRating = await reviewsQuery.AverageAsync(x => (float?)x.Rating, ct) ?? 0;


            var generatedAtUtc = DateTime.UtcNow;

            var reportData = new TherapistSessionReportData
            {
                TherapistId = therapist.Id,
                TherapistName =
                    therapist.User.Fullname ??
                    $"{therapist.User.Firstname} {therapist.User.Lastname}",
                Specialization = therapist.Specialization,
                ClientId = client.Id,
                ClientName = client.User.Fullname ?? $"{client.User.Firstname} {client.User.Lastname}",
                DateFrom = request.DateFrom.Date,
                DateTo = request.DateTo.Date,
                AppointmentsCount = appointmentsCount,
                CompletedSessionsCount = completedSessionsCount,
                AverageRating = (float)Math.Round(averageRating, 1),
                GeneratedAtUtc =  generatedAtUtc,
                Sessions = appointments
                            .Select(x => new TherapistSessionReportItemDto
                            {
                                ScheduledAtUtc = x.ScheduledAtUtc,
                                SessionType = GetSessionTypeName(x.SessionType),
                                Status = x.ScheduledAtUtc.AddHours(1) <= now ? "Completed" : "Upcoming"
                            }).ToList()
            };

            var pdfBytes = pdfService.GenerateSessionReport(reportData);

            var fileName = $"Bloomia-Report-" +
                            $"{request.ClientId}-" +
                            $"{request.DateFrom:yyyy-MM-dd}-" +
                            $"{request.DateTo:yyyy-MM-dd}.pdf";

            var savedFile = await fileStorageService.SaveReportAsync(pdfBytes, fileName, ct);

            var report = new TherapistReportEntity
            {
                TherapistId = therapist.Id,
                ClientId = request.ClientId,
                DateFrom = request.DateFrom.Date,
                DateTo = request.DateTo.Date,
                FilePath = savedFile.RelativePath,
                FileName = fileName,
                GeneratedAtUtc = generatedAtUtc,
                AppointmentsCount = appointmentsCount,
                CompletedSessionsCount = completedSessionsCount,
                AverageRating = (float)Math.Round(averageRating, 1),
                CreatedAtUtc = generatedAtUtc
            };

            ctx.TherapistReports.Add(report);

            await ctx.SaveChangesAsync(ct);
            
            return new TherapistReportListItemDto
            {
                Id = report.Id, 
                ClientId = report.ClientId,
                ClientName = reportData.ClientName,
                DateFrom = report.DateFrom,
                DateTo = report.DateTo,
                FileName = report.FileName,
                GeneratedAtUtc = report.GeneratedAtUtc,
                AppointmentsCount = report.AppointmentsCount,
                CompletedSessionsCount = report.CompletedSessionsCount,
                AverageRating = report.AverageRating
            };
        }
        private static string GetSessionTypeName(
        SessionType sessionType)
        {
            return sessionType switch
            {
                SessionType.VIDEO_CALL => "Video call",
                SessionType.CALL => "Call",
                SessionType.MESSAGE => "Chat",
                _ => "Unknown"
            };
        }
    }
}
