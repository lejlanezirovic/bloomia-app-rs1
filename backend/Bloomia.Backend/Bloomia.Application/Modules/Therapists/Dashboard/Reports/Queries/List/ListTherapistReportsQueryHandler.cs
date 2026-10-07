using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Reports.Queries.List
{
    public class ListTherapistReportsQueryHandler(IAppDbContext ctx, IAppCurrentUser currentUser) : IRequestHandler<ListTherapistReportsQuery, List<TherapistReportListItemDto>>
    {
        public async Task<List<TherapistReportListItemDto>> Handle(ListTherapistReportsQuery request, CancellationToken ct)
        {
            if (currentUser.UserId is null)
                throw new BloomiaBusinessRuleException("AUTH", "User is not authenticated");

            var therapistId = await ctx.Therapists
                .Where(x => x.UserId == currentUser.UserId)
                .Select(x => (int?)x.Id)
                .FirstOrDefaultAsync(ct);

            if (therapistId is null)
                throw new BloomiaBusinessRuleException("THERAPIST", "Therapist was not found.");


            return await ctx.TherapistReports
                .AsNoTracking()
                .Where(x =>
                    x.TherapistId == therapistId.Value)
                .OrderByDescending(x => x.GeneratedAtUtc)
                .Select(x => new TherapistReportListItemDto
                {
                    Id = x.Id, 
                    ClientId = x.ClientId,
                    ClientName = x.Client!.User.Fullname ?? $"{x.Client.User.Firstname} {x.Client.User.Lastname}",
                    DateFrom = x.DateFrom, 
                    DateTo = x.DateTo, 
                    FileName = x.FileName, 
                    GeneratedAtUtc = x.GeneratedAtUtc, 
                    AppointmentsCount = x.AppointmentsCount, 
                    CompletedSessionsCount = x.CompletedSessionsCount, 
                    AverageRating = x.AverageRating
                })
                .ToListAsync(ct);
        }
    }
}
