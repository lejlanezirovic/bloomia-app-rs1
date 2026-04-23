using Microsoft.AspNetCore.SignalR;

namespace Bloomia.API.Hubs
{
    [Authorize]
    public class ChatHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        public async Task JoinDirectChatGroup(int directChatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"directChat- {directChatId}");
        }
        public async Task LeaveDirectChatGroup(int directChatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"directChat- {directChatId}");
        }
    }
}
