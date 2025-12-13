using Bloomia.Application.Modules.DirectChat.Query.List.ListChatsForClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.List.ListChatsForTherapist
{
    public class ListDirectChatMessagesTherapistQueryHandler(IAppDbContext context) : IRequestHandler<ListDirectChatMessagesTherapistQuery, List<ListDirectChatMessagesTherapistQueryDto>>
    {
        public async Task<List<ListDirectChatMessagesTherapistQueryDto>> Handle(ListDirectChatMessagesTherapistQuery request, CancellationToken cancellationToken)
        {
            var therapist = await context.Therapists.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
            if (therapist == null)
            {
                throw new BloomiaNotFoundException("Therapist not found");
            }
            var directChats = context.DirectChats.Include(x => x.Client).ThenInclude(x=>x.User)
                    .Include(x => x.Therapist).Include(x => x.Messages)
                    .Where(x => x.TherapistId == therapist.Id)
                    .Select(x => new ListDirectChatMessagesTherapistQueryDto
                    {
                        DirectChatId = x.Id,
                        ClientId = x.ClientId,
                        ClientFullname = x.Client.User.Fullname,
                        ProfileImage = x.Client.User.ProfileImage,
                        IsLastMessageRead = x.Messages.OrderByDescending(x => x.SentAt).FirstOrDefault() != null ? x.Messages.OrderByDescending(x => x.SentAt).FirstOrDefault().isRead : true
                    }).AsNoTracking();

            return await directChats.ToListAsync(cancellationToken);
        }
    }
}
