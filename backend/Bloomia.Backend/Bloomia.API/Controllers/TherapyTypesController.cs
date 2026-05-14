using Bloomia.Application.Modules.TherapyTypes.Queries.List;

namespace Bloomia.API.Controllers
{
    [ApiController]
    [Route("api/therapy-types")]
    public class TherapyTypesController(ISender sender) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ListTherapyTypesQueryDto>>> List(CancellationToken ct)
        {
            var result = await sender.Send(new ListTherapyTypesQuery(), ct);
            return Ok(result);
        }
    }
}
