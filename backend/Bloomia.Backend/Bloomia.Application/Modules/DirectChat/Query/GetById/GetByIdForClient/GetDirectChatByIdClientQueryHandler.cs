using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.NewFolder.GetById
{
    public class GetDirectChatByIdClientQueryHandler(IAppDbContext context) : IRequestHandler<GetDirectChatByIdClientQuery, GetDirectChatByIdClientQueryDto>
    {
        public async Task<GetDirectChatByIdClientQueryDto> Handle(GetDirectChatByIdClientQuery request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            if (client == null)
            {
                throw new Exception("Client or Therapist not found");
            }

            var chat = await context.DirectChats.Include(x => x.Client).Include(x => x.Therapist).ThenInclude(x => x.User)
                    .Include(x=>x.Messages)
                    .FirstOrDefaultAsync(x => x.Id == request.DirectChatId && x.ClientId == client.Id, cancellationToken);

            if (chat == null) {
                throw new BloomiaNotFoundException("chat not found");
            }

            foreach(var i in chat.Messages)
            {
                if (!i.isRead && i.SenderId != client.Id) ////da ne bi oznacavala svoje poruke kao procitane
                {
                    i.isRead = true;
                }        
            }
            await context.SaveChangesAsync(cancellationToken);

            var dto = new GetDirectChatByIdClientQueryDto
            {
                TherapistId = chat.TherapistId,
                TherapistFullname = chat.Therapist.User.Fullname,
                ProfileImage = chat.Therapist.User.ProfileImage
            };

            foreach(var i in chat.Messages)
            {
                var msg = new MessageDto
                {   
                    MessageId = i.Id,
                    SenderId = i.SenderId,
                    SenderType=i.SenderType.ToString(),
                    Content = i.Content,
                    SentAt = i.SentAt,
                    IsRead = i.isRead
                };
                dto.Messages.Add(msg);
            }
            return dto;
        }
    }
}
