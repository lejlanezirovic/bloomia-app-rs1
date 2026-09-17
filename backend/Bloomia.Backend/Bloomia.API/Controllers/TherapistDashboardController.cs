using Bloomia.Application.Modules.Therapists.Dashboard.Queries.Overview;
using Bloomia.Application.Modules.Therapists.Dashboard.Queries.Reviews;

namespace Bloomia.API.Controllers
{
    [ApiController]
    [Route("api/therapist-dashboard")]
    public class TherapistDashboardController(ISender sender) : ControllerBase
    {
        [HttpGet("overview")]
        public async Task<TherapistDashboardOverviewDto> GetOverview(CancellationToken ct)
        {
            return await sender.Send(new GetTherapistDashboardOverviewQuery(), ct);
        }

        [HttpGet("reviews")]
        public async Task<TherapistDashboardReviewsDto> GetReviews(CancellationToken ct)
        {
            return await sender.Send( new GetTherapistDashboardReviewsQuery(), ct);
        }
    }
}
