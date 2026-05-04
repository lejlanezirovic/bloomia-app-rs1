using Bloomia.Application.Modules.TherapistAvailability.Command.Create;
using Bloomia.Application.Modules.TherapistAvailability.Command.Delete.DeleteTimeByDate;
using Bloomia.Application.Modules.TherapistAvailability.Command.Update;
using Bloomia.Application.Modules.TherapistAvailability.Query.List;
using Bloomia.Application.Modules.TherapistAvailability.Query.ListTherapistTimesByDate;
using Bloomia.Application.Modules.TherapistAvailability.Query.ListBookedTimesByDate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bloomia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistAvailabilityController(ISender sender) : ControllerBase
    {
        [Authorize(Roles ="THERAPIST")]
        [HttpPost("create-my-working-time")]
        public async Task<ActionResult<CreateTherapistAvailabilityCommandDto>> CreateTherapistAvailability([FromBody] CreateTherapistAvailabilityCommand request,CancellationToken ct)
        {
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpGet("get-available-times-by-date")]
        public async Task<ActionResult<ListAvailableTimesByDateQueryDto>> GetAvailableTimesForTherapist(DateOnly date, CancellationToken ct)
        {
            var request = new ListAvailableTimesByDateQuery();
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            request.Date = date;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpGet("get-all-working-times-by-date")]
        public async Task<ActionResult<ListTherapistTimesByDateQueryDto>> GetAllTimeSlotsByDate(DateOnly date, CancellationToken ct)
        {
            var request = new ListTherapistTimesByDateQuery();
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            request.Date = date;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpGet("get-booked-times-by-date")]
        public async Task<ActionResult<ListBookedTimesByDateQueryDto>> GetBookedlTimeSlotsByDate(DateOnly date, CancellationToken ct)
        {
            var request = new ListBookedTimesByDateQuery();
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            request.Date = date;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpDelete("remove-working-time-from-date")]
        public async Task<ActionResult<string>> RemoveTimeFromDate([FromBody]DeleteTherapistAvailableTimeByDateCommand request, CancellationToken ct)
        {
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            var result=await sender.Send(request, ct);
            return Ok(result);
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpPut("update-my-working-time")]
        public async Task<ActionResult<UpdateTherapistTimeCommandDto>> UpdateTimeForTherapist([FromBody] UpdateTherapistTimeCommand request, CancellationToken ct)
        {
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return Ok(result);
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpGet("list-my-working-dates-and-times")]
        public async Task<ActionResult<ListAllTherapistAvailabilitiesQueryDto>> UpdateTimeForTherapist( CancellationToken ct)
        {
            var request =new ListAllTherapistAvailabilitiesQuery();
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return Ok(result);
        }

        [Authorize(Roles = "CLIENT")]
        [HttpGet("therapists/{therapistId}/working-dates-and-times")]
        public async Task<ActionResult<ListAllTherapistAvailabilitiesQueryDto>> GetWorkingDatesAndTimesForClient(int therapistId, CancellationToken ct)
        {
            var result = await sender.Send(new ListTherapistAvailabilitiesForClientQuery
            {
                TherapistId = therapistId
            }, ct);

            return Ok(result);
        }
    }
}
