using Bloomia.Application.Abstractions;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Services
{
    public class PushNotificationService:IPushNotificationService
    {
        public async Task<string> SendJournalReminderAsync(string token,CancellationToken ct)
        {
            var msg = new Message
            {
                Token = token,
                Notification = new Notification
                {
                    Title = "Bloomia Journal Reminder",
                    Body = "Jesi li ispunio svoj journal za danas?"
                }
            };
            return await FirebaseMessaging.DefaultInstance.SendAsync(msg, ct);
        }
    }
}
