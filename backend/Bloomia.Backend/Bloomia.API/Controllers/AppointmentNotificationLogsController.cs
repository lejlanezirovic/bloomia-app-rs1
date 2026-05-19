using Bloomia.Application.Modules.AppointmentNotificationLogs.Queries.List;

namespace Bloomia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentNotificationLogsController(ISender sender) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<ListAppointmentNotificationLogsQueryDto>>> List(CancellationToken ct)
        {
            var result = await sender.Send(new ListAppointmentNotificationLogsQuery(), ct);
            return Ok(result);
        }
    }
}
