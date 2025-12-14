using Bloomia.Application.Modules.Reviews.Commands.Create;
using Bloomia.Application.Modules.Reviews.Query.GetByTherapistId;

namespace Bloomia.API.Controllers
{
    [ApiController]
    [Route("api/Reviews")]
    public sealed class ReviewsController(ISender sender) : ControllerBase
    {
        [HttpGet("therapists/{therapistId}/reviews")]
        [Authorize(Roles = "CLIENT,THERAPIST")]
        public async Task<PageResult<GetReviewsByTherapistIdQueryDto>> GetReviewsOfTherapist(int therapistId, [FromQuery] PageRequest paging, CancellationToken ct)
        {
            var result = await sender.Send(new GetReviewsByTherapistIdQuery
            {
                TherapistId = therapistId,
                Paging = paging
            }, ct);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "CLIENT")]
        public async Task<ActionResult<int>> CreateReview(CreateReviewCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return Ok(new { id });
        }
    }
}
