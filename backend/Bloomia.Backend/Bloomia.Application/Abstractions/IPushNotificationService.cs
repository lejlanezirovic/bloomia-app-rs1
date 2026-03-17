using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Abstractions
{
    public interface IPushNotificationService
    {
        Task<string> SendJournalReminderAsync(string token, CancellationToken ct);
    }
}
