using Bloomia.Application.Modules.Therapists.Dashboard.Queries.Overview;
using Bloomia.Application.Modules.Therapists.Dashboard.Queries.Reviews;
using Bloomia.Application.Modules.Therapists.Dashboard.Reports.Commands.Generate;
using Bloomia.Application.Modules.Therapists.Dashboard.Reports.Queries.GetFile;
using Bloomia.Application.Modules.Therapists.Dashboard.Reports.Queries.List;

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

        [HttpPost("reports/generate")]
        public async Task<TherapistReportListItemDto> GenerateReport(CancellationToken ct)
        {
            return await sender.Send(new GenerateTherapistReportCommand(), ct);
        }

        [HttpGet("reports")]
        public async Task<List<TherapistReportListItemDto>> GetReports(
            CancellationToken ct)
        {
            return await sender.Send(new ListTherapistReportsQuery(), ct);
        }

        [HttpGet("reports/{id:int}/file")]
        public async Task<IActionResult> GetReportFile(int id, CancellationToken ct)
        {
            var report = await sender.Send(new GetTherapistReportFileQuery
                    {
                        Id = id
                    }, ct);

            return File(report.Content, "application/pdf", report.FileName);
        }
    }
}
