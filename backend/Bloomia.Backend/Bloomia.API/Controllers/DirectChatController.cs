using Bloomia.Application.Modules.DirectChat.Command.Create.CreateForClient;
using Bloomia.Application.Modules.DirectChat.Command.Create.CreateForTherapist;
using Bloomia.Application.Modules.DirectChat.Command.Delete.DeleteForClient;
using Bloomia.Application.Modules.DirectChat.Command.Delete.DeleteForTherapist;
using Bloomia.Application.Modules.DirectChat.Command.Update.UpdateForClient;
using Bloomia.Application.Modules.DirectChat.Command.Update.UpdateForTherapist;
using Bloomia.Application.Modules.DirectChat.Query.List.ListChatsForClient;
using Bloomia.Application.Modules.DirectChat.Query.List.ListChatsForTherapist;
using Bloomia.Application.Modules.DirectChat.Query.NewFolder.GetById;
using Bloomia.Application.Modules.DirectChat.Query.NewFolder.GetByIdForTherapist;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bloomia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectChatController(ISender sender) : ControllerBase
    {
        [Authorize(Roles = "CLIENT")]
        [HttpPost("client/messages/send")]
        public async Task<ActionResult<SendMessageCommandDto>> SendMessageForClient([FromBody] SendMessageCommand command, CancellationToken ct)
        {  
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId=int.Parse(userClaim!.Value);
            command.UserId = userId;
            var result=await sender.Send(command, ct);
            return result;
        }  
        [Authorize(Roles = "CLIENT")]
        [HttpGet("client/direct-chats")]
        public async Task<ActionResult<List<ListDirectChatMessagesQueryDto>>> ListDirectChatsForClient(CancellationToken ct)
        {
            var request = new ListDirectChatMessagesQuery();
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "CLIENT")]
        [HttpGet("client/direct-chat/{directChatId:int}")]
        public async Task<ActionResult<GetDirectChatByIdClientQueryDto>> GetChatByIdForClient (int directChatId, CancellationToken ct)
        {
            var request = new GetDirectChatByIdClientQuery
            {
                DirectChatId = directChatId
            };
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "CLIENT")]
        [HttpDelete("client/direct-chat/messages/delete/{messageId:int}")]
        public async Task<ActionResult<DeleteMessageCommandDto>> DeleteMessageForClient(int messageId, CancellationToken ct)
        {
            var request=new DeleteMessageCommand
            {
                MessageId = messageId
            };
            var userClaim=User.FindFirst("id")?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId=int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result=await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "CLIENT")]
        [HttpPut("client/messages/update-msgById")]
        public async Task<ActionResult<UpdateMessageCommandDto>> UpdateMessageForClient([FromBody] UpdateMessageCommand request, CancellationToken ct)
        {
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpPost("therapist/messages/send")]
        public async Task<ActionResult<SendMessageTherapistCommandDto>> SendMessageFromTherapistToClient([FromBody] SendMessageTherapistCommand command, CancellationToken ct)
        {
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            command.UserId = userId;
            var result = await sender.Send(command, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpGet("therapist/direct-chats")]
        public async Task<ActionResult<List<ListDirectChatMessagesTherapistQueryDto>>> ListDirectChatsForTherapist(CancellationToken ct)
        {
            var request = new ListDirectChatMessagesTherapistQuery();
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpGet("therapist/direct-chat/{directChatId:int}")]
        public async Task<ActionResult<GetDirectChatByIdTherapistQueryDto>> GetChatByIdForTherapist(int directChatId, CancellationToken ct)
        {
            var request = new GetDirectChatByIdTherapistQuery
            {
                DirectChatId = directChatId
            };
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpDelete("therapist/direct-chat/messages/delete/{messageId:int}")]
        public async Task<ActionResult<DeleteMessageTherapistCommandDto>> DeleteMessageForTherapist(int messageId, CancellationToken ct)
        {
            var request = new DeleteMessageTherapistCommand
            {
                MessageId = messageId
            };
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpPut("therapist/messages/update-msgById")]
        public async Task<ActionResult<UpdateMessageTherapistCommandDto>> UpdateMessageForTherapist([FromBody] UpdateMessageTherapistCommand request, CancellationToken ct)
        {
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
    }
}
