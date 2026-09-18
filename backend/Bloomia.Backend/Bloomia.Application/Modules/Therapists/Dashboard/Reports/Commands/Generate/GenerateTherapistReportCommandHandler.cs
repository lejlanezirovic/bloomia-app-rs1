using Bloomia.Application.Modules.Therapists.Dashboard.Reports.Queries.List;
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

            var therapist = await ctx.Therapists.AsNoTracking()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == currentUser.UserId, ct);

            if (therapist == null)
                throw new BloomiaBusinessRuleException("THERAPIST", "Therapist was not found.");

            var now = DateTime.UtcNow;

            var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            var nextMonth = monthStart.AddMonths(1);

            var appointmentsQuery = ctx.Appointments.AsNoTracking()
                .Where(x =>
                    x.TherapistAvailability.TherapistId == therapist.Id &&
                    x.ScheduledAtUtc >= monthStart &&
                    x.ScheduledAtUtc < nextMonth);

            var appointmentsCount = await appointmentsQuery.CountAsync(ct);

            var completedSessionsCount = await appointmentsQuery
                .CountAsync(x => x.ScheduledAtUtc.AddHours(1) <= now, ct);

            var activeClientsCount = await appointmentsQuery
                .Select(x => x.ClientId)
                .Distinct().CountAsync(ct);

            var reviewsQuery = ctx.Reviews.AsNoTracking()
                .Where(x => x.Appointment.TherapistAvailability.TherapistId == therapist.Id &&
                            x.CreatedAtUtc >= monthStart && x.CreatedAtUtc < nextMonth);

            var totalReviews = await reviewsQuery.CountAsync(ct);

            var averageRating = await reviewsQuery.AverageAsync(x => (float?)x.Rating, ct) ?? 0;

            var fiveStarReviews =
                await reviewsQuery
                    .CountAsync(x => x.Rating == 5, ct);

            var fourStarReviews =
                await reviewsQuery
                    .CountAsync(x => x.Rating == 4, ct);

            var threeStarReviews =
                await reviewsQuery
                    .CountAsync(x => x.Rating == 3, ct);

            var twoStarReviews =
                await reviewsQuery
                    .CountAsync(x => x.Rating == 2, ct);

            var oneStarReviews =
                await reviewsQuery
                    .CountAsync(x => x.Rating == 1, ct);

            var generatedAtUtc = DateTime.UtcNow;

            var reportData = new TherapistMonthlyReportData
            {
                TherapistName =
                    therapist.User.Fullname ??
                    $"{therapist.User.Firstname} {therapist.User.Lastname}",
                Specialization =
                    therapist.Specialization,

                Month = now.Month,
                Year = now.Year,

                AppointmentsCount =
                    appointmentsCount,

                CompletedSessionsCount =
                    completedSessionsCount,

                ActiveClientsCount =
                    activeClientsCount,

                AverageRating =
                    (float)Math.Round(averageRating, 1),
                TotalReviews =
                    totalReviews,

                FiveStarReviews =
                    fiveStarReviews,

                FourStarReviews =
                    fourStarReviews,

                ThreeStarReviews =
                    threeStarReviews,

                TwoStarReviews =
                    twoStarReviews,

                OneStarReviews =
                    oneStarReviews,

                GeneratedAtUtc =
                    generatedAtUtc
            };

            var pdfBytes = pdfService.GenerateMonthlyReport(reportData);

            var fileName = $"Bloomia-Monthly-Report-{now:yyy-MM}.pdf";

            var savedFile = await fileStorageService.SaveReportAsync(pdfBytes, fileName, ct);

            var existingReport = await ctx.TherapistReports.FirstOrDefaultAsync(
                x => x.TherapistId == therapist.Id &&
                     x.Month == now.Month &&
                     x.Year == now.Year, ct);

            string? oldFilePath = null;

            if(existingReport == null)
            {
                existingReport = new TherapistReportEntity
                {
                    TherapistId = therapist.Id,
                    Month = now.Month,
                    Year = now.Year,
                    CreatedAtUtc = generatedAtUtc
                };

                ctx.TherapistReports.Add(existingReport);
            }
            else
            {
                oldFilePath = existingReport.FilePath;
                existingReport.ModifiedAtUtc = generatedAtUtc;
            }

            existingReport.FilePath = savedFile.RelativePath;

            existingReport.FileName = fileName;

            existingReport.GeneratedAtUtc = generatedAtUtc;

            existingReport.AppointmentsCount = appointmentsCount;

            existingReport.CompletedSessionsCount = completedSessionsCount;

            existingReport.ActiveClientsCount = activeClientsCount;

            existingReport.AverageRating = (float)Math.Round(averageRating, 1);

            existingReport.TotalReviews = totalReviews;

            await ctx.SaveChangesAsync(ct);

            if (!string.IsNullOrWhiteSpace(oldFilePath) && oldFilePath != savedFile.RelativePath)
                fileStorageService.DeleteReportIfExists(oldFilePath);

            return new TherapistReportListItemDto
            {
                Id = existingReport.Id,

                Month = existingReport.Month,
                Year = existingReport.Year,

                FileName = existingReport.FileName,

                GeneratedAtUtc = existingReport.GeneratedAtUtc,

                AppointmentsCount = existingReport.AppointmentsCount,

                CompletedSessionsCount = existingReport.CompletedSessionsCount,

                ActiveClientsCount = existingReport.ActiveClientsCount,

                AverageRating = existingReport.AverageRating,

                TotalReviews = existingReport.TotalReviews
            };

        }
    }
}
