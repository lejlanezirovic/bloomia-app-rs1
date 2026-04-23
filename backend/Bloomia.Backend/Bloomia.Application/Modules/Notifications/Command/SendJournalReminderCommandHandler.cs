using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Notifications.Command
{
    public class SendJournalReminderCommandHandler(IAppDbContext context, IPushNotificationService pushNotificationService) : IRequestHandler<SendJournalReminderCommand, string>
    {
        public async Task<string> Handle(SendJournalReminderCommand request, CancellationToken cancellationToken)
        {
            var activeToken = await context.NotificationTokens
                        .Where(x => x.UserId == request.UserId && x.IsActive)
                        .OrderByDescending(x => x.CreatedAt)
                        .Select(x => x.Token)
                        .FirstOrDefaultAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(activeToken)) {
                throw new BloomiaBusinessRuleException("notification token", "NO active notification token found for this user");
            
            }
            var msgId=await pushNotificationService.SendJournalReminderAsync(activeToken , cancellationToken);
            return msgId;
        }
    }
}
