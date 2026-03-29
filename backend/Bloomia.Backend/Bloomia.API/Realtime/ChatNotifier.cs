using Bloomia.API.Hubs;
using Bloomia.Application.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace Bloomia.API.Realtime
{
    public class ChatNotifier(IHubContext<ChatHub> hubContext) : IChatNotifier
    {
        public async Task NotifyMessageReceivedAsync(string receiverUserId, object payload, CancellationToken cancellationToken = default)
        {
            await hubContext.Clients
            .User(receiverUserId)
            .SendAsync("ReceiveMessage", payload, cancellationToken); 
        }
        public async Task NotifyUserAsync(string userId, string eventName, object payload, CancellationToken cancellationToken = default)
        {
            await hubContext.Clients
            .User(userId)
            .SendAsync(eventName, payload, cancellationToken);
        }
        public async Task NotifyUsersInChatAsync(int directChatId, string eventName, object payload, CancellationToken cancellationToken = default)
        {
            await hubContext.Clients
           .Group($"directchat-{directChatId}")
           .SendAsync(eventName, payload, cancellationToken);
        }
    }
}
