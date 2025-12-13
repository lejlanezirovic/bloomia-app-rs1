using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.List.ListChatsForClient
{
    public class ListDirectChatMessagesQueryHandler(IAppDbContext context) : IRequestHandler<ListDirectChatMessagesQuery, List<ListDirectChatMessagesQueryDto>>
    {
        public async Task<List<ListDirectChatMessagesQueryDto>> Handle(ListDirectChatMessagesQuery request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
            if (client == null)
            {
                throw new BloomiaNotFoundException("Client not found");
            }
            var directChats = context.DirectChats.Include(x => x.Client)
                    .Include(x => x.Therapist).ThenInclude(x => x.User).Include(x => x.Messages)
                    .Where(x => x.ClientId == client.Id)
                    .Select(x => new ListDirectChatMessagesQueryDto
                    {
                        DirectChatId = x.Id,
                        TherapistId = x.TherapistId,
                        TherapistFullname = x.Therapist.User.Fullname,
                        ProfileImage = x.Therapist.User.ProfileImage,
                        IsReadLAstMessage = x.Messages.OrderByDescending(x => x.SentAt).FirstOrDefault() != null ? x.Messages.OrderByDescending(x => x.SentAt).FirstOrDefault().isRead : true
                    }).AsNoTracking();  

            return await directChats.ToListAsync(cancellationToken);
        }
    }
}
