using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Queries.Reviews
{
    public class GetTherapistDashboardReviewsQueryHandler(IAppDbContext ctx, IAppCurrentUser currentUser) 
        : IRequestHandler<GetTherapistDashboardReviewsQuery, TherapistDashboardReviewsDto>
    {
        public async Task<TherapistDashboardReviewsDto> Handle(GetTherapistDashboardReviewsQuery request, CancellationToken ct)
        {
            var userId = currentUser.UserId;

            if (userId is null)
                throw new BloomiaBusinessRuleException("AUTH", "User is not authenticated.");

            var therapistId = await ctx.Therapists
                .Where(x => x.UserId == userId)
                .Select(x => (int?)x.Id).FirstOrDefaultAsync(ct);

            if (therapistId is null)
                throw new BloomiaBusinessRuleException("THERAPIST",
                    "Therapist was not found.");

            var reviewsQuery = ctx.Reviews
                .AsNoTracking().Where(x => x.Appointment.TherapistAvailability.TherapistId == therapistId.Value);

            var totalReviews = await reviewsQuery.CountAsync(ct);
            var fiveStarReviews = await reviewsQuery.CountAsync(x => x.Rating == 5, ct);
            var fourStarReviews = await reviewsQuery.CountAsync(x => x.Rating == 4, ct);
            var threeStarReviews = await reviewsQuery.CountAsync(x => x.Rating == 3, ct);
            var twoStarReviews = await reviewsQuery.CountAsync(x => x.Rating == 2, ct);
            var oneStarReviews = await reviewsQuery.CountAsync(x => x.Rating == 1, ct);

            var latestReviews = await reviewsQuery.OrderByDescending(x => x.CreatedAtUtc)
                .Take(3)
                .Select(x => new TherapistDashboardReviewItemDto
                {
                    Id = x.Id,
                    Rating = x.Rating,
                    ClientName = x.Appointment.Client.User.Firstname + " " + x.Appointment.Client.User.Lastname,
                    Comment = x.Comment,
                    CreatedAtUtc = x.CreatedAtUtc,
                })
                .ToListAsync(ct);

            return new TherapistDashboardReviewsDto
            {
                TotalReviews = totalReviews,
                FiveStarReviews = fiveStarReviews,
                FourStarReviews = fourStarReviews,
                ThreeStarReviews = threeStarReviews,
                TwoStarReviews = twoStarReviews,
                OneStarReviews = oneStarReviews,
                LatestReviews = latestReviews,
            };
        }
    }
}
