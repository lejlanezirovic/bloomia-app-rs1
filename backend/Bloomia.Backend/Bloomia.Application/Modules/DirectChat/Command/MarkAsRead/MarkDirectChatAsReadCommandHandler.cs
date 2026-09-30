using Bloomia.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.MarkAsRead
{
    public class MarkDirectChatAsReadCommandHandler(IAppDbContext context) : IRequestHandler<MarkDirectChatAsReadCommand>
    {
        public async Task Handle(MarkDirectChatAsReadCommand request, CancellationToken ct)
        {
            var therapist = await context.Therapists.FirstOrDefaultAsync(x => x.UserId == request.UserId, ct);

            var client = await context.Clients.FirstOrDefaultAsync(x => x.UserId == request.UserId, ct);

            if (therapist == null && client == null)
                throw new BloomiaNotFoundException("User not found.");

            var directChat = await context.DirectChats
                .FirstOrDefaultAsync(x => x.Id == request.DirectChatId, ct);

            if (directChat == null)
                throw new BloomiaNotFoundException("Direct chat not found.");

            SenderType senderTypeToMark;

            if(therapist != null)
            {
                if (directChat.TherapistId != therapist.Id)
                    throw new BloomiaNotFoundException("Direct chat not found.");

                senderTypeToMark = SenderType.CLIENT;
            }
            else
            {
                if (directChat.ClientId != client!.Id)
                    throw new BloomiaNotFoundException("Drect chat not found");
                senderTypeToMark = SenderType.THERAPIST;
            }

            var unreadMessages = await context.Messages
                .Where(x => x.DirectChatId == request.DirectChatId &&
                        x.SenderType == senderTypeToMark &&
                        !x.isRead)
                .ToListAsync(ct);

            foreach (var message in unreadMessages)
                message.isRead = true;

            await context.SaveChangesAsync(ct);
        }
    }
}
