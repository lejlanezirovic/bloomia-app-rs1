using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Sessions
{
    public class ChatHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // ovdje SignalR čita NameIdentifier
            Console.WriteLine("Client connected: " + Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

    }
}
