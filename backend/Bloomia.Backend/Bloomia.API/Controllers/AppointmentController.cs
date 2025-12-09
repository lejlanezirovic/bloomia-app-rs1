using Bloomia.Application.Modules.Appointments.Command.Create;
using Bloomia.Application.Modules.Appointments.Command.Delete;
using Bloomia.Application.Modules.Appointments.Query.List;
using Bloomia.Domain.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bloomia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(ISender sender) : ControllerBase
    {
        [Authorize(Roles ="CLIENT")]
        [HttpPost("create-appointment")]
        public async Task<ActionResult<CreateAppointmentCommandDto>> CreateAppointmentForClient(int TAid, SessionType sessionType, CancellationToken ct)
        {
            var request = new CreateAppoinmentCommand
            {
                TherapistAvailabilityId = TAid,
                SessionType = sessionType
            };
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId=int.Parse(userClaim?.Value);
            request.UserId = userId;
            var result=await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "CLIENT")]
        [HttpGet("get-my-appointments")]
        public async Task<ActionResult<List<ListAppointmentsQueryDto>>> GetClientAppointments (CancellationToken ct)
        {
            var request = new ListAppointmentsQuery();
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim?.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "CLIENT, THERAPIST")]
        [HttpDelete("delete-appointment")]
        public async Task<ActionResult<string>> DeleteAppointmentForClientAndTherapist(int id, CancellationToken ct)
        {
            var request = new DeleteAppointmentCommand
            {
                AppointmentId = id
            };
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim?.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }

    }
}
