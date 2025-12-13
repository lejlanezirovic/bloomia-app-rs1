using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Update.UpdateForClient
{
    public class UpdateMessageCommandHandler(IAppDbContext context) : IRequestHandler<UpdateMessageCommand, UpdateMessageCommandDto>
    {
        public async Task<UpdateMessageCommandDto> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            if (client == null)
            {
                throw new BloomiaNotFoundException("Client not found");
            }
            var message = await context.Messages.Include(x => x.DirectChat)
                .FirstOrDefaultAsync(x => x.Id == request.MessageId && x.DirectChatId == request.DirectChatId
                                    && x.DirectChat.ClientId == client.Id, cancellationToken);
            if (message == null || message.IsDeleted)
            {
               throw new BloomiaNotFoundException("Message not found");
            }
            if(message.SenderId!=client.Id)
            {
                throw new BloomiaConflictException("You can not update this message");
            }
            var dto=new UpdateMessageCommandDto
            {
                DirectChatId=message.DirectChatId.Value,
                MessageId=message.Id,
                OldMessage=message.Content,
                UpdatedContent=request.NewContent,
                IsRead=message.isRead
            };
            message.Content=request.NewContent;
            await context.SaveChangesAsync(cancellationToken);
            return dto;
        }
    }
}
