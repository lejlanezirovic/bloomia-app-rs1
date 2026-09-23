using Bloomia.Application.Modules.AppointmentNotificationLogs.Queries.List;

namespace Bloomia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentNotificationLogsController(ISender sender) : ControllerBase
    {
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<PageResult<ListAppointmentNotificationLogsQueryDto>> List([FromQuery] ListAppointmentNotificationLogsQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }
    }
}
