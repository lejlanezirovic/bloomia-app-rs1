using Bloomia.Application.Modules.Notifications.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bloomia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationTokenController(ISender sender) : ControllerBase
    {
        [Authorize(Roles ="CLIENT")]
        [HttpPost("register-notification-token")]
        public async Task<IActionResult> RegisterToken([FromBody] RegisterNotificationTokenCommand request,  CancellationToken ct)
        {
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);

            request.UserId = userId;

            await sender.Send(request, ct);
            return Ok();
        }
        [Authorize(Roles = "CLIENT")]
        [HttpPost("journal-reminder")]
        public async Task<ActionResult<string>> SendJournalReminder(CancellationToken ct)
        {
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            var command = new SendJournalReminderCommand
            {
                UserId = userId
            };
            var result=await sender.Send(command, ct);
            return result;
        }
    }
}
