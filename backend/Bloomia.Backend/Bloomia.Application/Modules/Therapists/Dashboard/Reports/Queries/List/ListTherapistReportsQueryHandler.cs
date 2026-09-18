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
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .Select(x =>
                    new TherapistReportListItemDto
                    {
                        Id = x.Id,

                        Month = x.Month,
                        Year = x.Year,

                        FileName = x.FileName,

                        GeneratedAtUtc =
                            x.GeneratedAtUtc,

                        AppointmentsCount =
                            x.AppointmentsCount,

                        CompletedSessionsCount =
                            x.CompletedSessionsCount,

                        ActiveClientsCount =
                            x.ActiveClientsCount,

                        AverageRating =
                            x.AverageRating,

                        TotalReviews =
                            x.TotalReviews
                    })
                .ToListAsync(ct);
        }
    }
}
