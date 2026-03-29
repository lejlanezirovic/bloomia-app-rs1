using Bloomia.Application.Abstractions;
using Bloomia.Application.Modules.DirectChat.Query.NewFolder.GetById;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.NewFolder.GetByIdForTherapist
{
    public class GetDirectChatByIdTherapistQueryHandler(IAppDbContext context , IChatNotifier chatNotifier) : IRequestHandler<GetDirectChatByIdTherapistQuery, GetDirectChatByIdTherapistQueryDto>
    {
        public async Task<GetDirectChatByIdTherapistQueryDto> Handle(GetDirectChatByIdTherapistQuery request, CancellationToken cancellationToken)
        {
            var therapist = await context.Therapists.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            if (therapist == null)
            {
                throw new Exception("Therapist not found");
            }

            var chat = await context.DirectChats.Include(x => x.Client).ThenInclude(x => x.User)
                    .Include(x => x.Therapist)
                    .Include(x => x.Messages)
                    .FirstOrDefaultAsync(x => x.Id == request.DirectChatId && x.TherapistId == therapist.Id, cancellationToken);

            if (chat == null)
            {
                throw new BloomiaNotFoundException("chat not found");
            }

            var readMessageIds = new List<int>();
            foreach (var i in chat.Messages)
            {
                if (!i.isRead && i.SenderId != therapist.Id)////da ne bi oznacavala svoje poruke kao procitane
                {
                    i.isRead = true;
                    readMessageIds.Add(i.Id);
                }
            }
            await context.SaveChangesAsync(cancellationToken);

            var dto = new GetDirectChatByIdTherapistQueryDto
            {
                DirectChatId=chat.Id,
                ClientId = chat.ClientId,
                ClientFullname = chat.Client.User.Fullname,
                ProfileImage = chat.Client.User.ProfileImage
            };

            foreach (var i in chat.Messages)
            {
                var msg = new MessageDto
                {
                    MessageId = i.Id,
                    SenderId = i.SenderId,
                    SenderType = i.SenderType.ToString(),
                    Content = i.Content,
                    SentAt = i.SentAt,
                    IsRead = i.isRead
                };
                dto.Messages.Add(msg);
            }
            if (readMessageIds.Any())
            {
                await chatNotifier.NotifyUserAsync(chat.Client.User.Id.ToString(), "MessagesRead",
                   new
                   {
                       DirectChatId = chat.Id,
                       MessageIds = readMessageIds
                   }, cancellationToken
               );
            }
            return dto;
        }
    }
}
