using Bloomia.Application.Modules.Moods.Commands.Create;
using Bloomia.Application.Modules.Moods.Commands.Delete;
using Bloomia.Application.Modules.Moods.Queries.List;

namespace Bloomia.API.Controllers
{
    [ApiController]
    [Route("api/MoodEntries")]
    public sealed class MoodsController(ISender sender) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "CLIENT,THERAPIST")]
        public async Task<PageResult<ListMoodEntriesQueryDto>> ListMoodEntries([FromQuery] ListMoodEntriesQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "CLIENT")]
        public async Task<ActionResult<int>> CreateMoodEntry(CreateMoodEntryCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return Ok(new { id });
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "CLIENT")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteMoodEntryCommand { Id = id }, ct);
            return NoContent();
        }
    }
}
