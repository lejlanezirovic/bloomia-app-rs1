using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Abstractions
{
    public interface IChatNotifier
    {
        Task NotifyMessageReceivedAsync(string receiverUserId, object payload, CancellationToken cancellationToken = default);

        Task NotifyUsersInChatAsync( int directChatId, string eventName,object payload, CancellationToken cancellationToken = default);

        Task NotifyUserAsync(string userId, string eventName, object payload, CancellationToken cancellationToken = default);
    }
}
