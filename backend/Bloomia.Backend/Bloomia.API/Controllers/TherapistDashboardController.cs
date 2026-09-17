using Bloomia.Application.Modules.Therapists.Dashboard.Queries.Overview;

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
    }
}
