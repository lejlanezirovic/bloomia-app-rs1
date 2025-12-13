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
        [HttpPost("send-direct-message")]
        public async Task<ActionResult<SendMessageCommandDto>> SendMessageForClient(int therapistID, string content, CancellationToken ct)
        {
            var request = new SendMessageCommand
            {
                TherapistId = therapistID,
                Content = content,
            };
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId=int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result=await sender.Send(request, ct);
            return result;

        }  
        [Authorize(Roles = "CLIENT")]
        [HttpGet("Chats")]
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
        [HttpGet("Get-Chat-by-id")]
        public async Task<ActionResult<GetDirectChatByIdClientQueryDto>> GetChatByIdForClient (int chatId, CancellationToken ct)
        {
            var request = new GetDirectChatByIdClientQuery
            {
                DirectChatId = chatId
            };
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "CLIENT")]
        [HttpDelete("delete-msgById")]
        public async Task<ActionResult<DeleteMessageCommandDto>> DeleteMessageForClient(int DirectChatId, int MessageId, CancellationToken ct)
        {
            var request=new DeleteMessageCommand
            {
                DirectChatId = DirectChatId,
                MessageId = MessageId
            };
            var userClaim=User.FindFirst("id")?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId=int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result=await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "CLIENT")]
        [HttpPut("update-msgById")]
        public async Task<ActionResult<UpdateMessageCommandDto>> UpdateMessageForClient([FromBody] UpdateMessageCommand request, CancellationToken ct)
        {
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }

        [Authorize(Roles = "THERAPIST")]
        [HttpPost("therapist-send-direct-message")]
        public async Task<ActionResult<SendMessageTherapistCommandDto>> SendMessageFromTherapistToClient(int ClientId, string content, CancellationToken ct)
        {
            var request = new SendMessageTherapistCommand
            {
                ClientId = ClientId,
                Content = content
            };
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpGet("therapist-chats")]
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
        [HttpGet("therapist-get-chat-by-id")]
        public async Task<ActionResult<GetDirectChatByIdTherapistQueryDto>> GetChatByIdForTherapist(int chatId, CancellationToken ct)
        {
            var request = new GetDirectChatByIdTherapistQuery
            {
                DirectChatId = chatId
            };
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpDelete("therapist-delete-msgById")]
        public async Task<ActionResult<DeleteMessageTherapistCommandDto>> DeleteMessageForTherapist(int DirectChatId, int MessageId, CancellationToken ct)
        {
            var request = new DeleteMessageTherapistCommand
            {
                DirectChatId = DirectChatId,
                MessageId = MessageId
            };
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);
            request.UserId = userId;
            var result = await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "THERAPIST")]
        [HttpPut("update-therapist-msgById")]
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
