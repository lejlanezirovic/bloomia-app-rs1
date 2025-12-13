using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Delete.DeleteForClient
{
    public class DeleteMessageCommandHandler(IAppDbContext context) : IRequestHandler<DeleteMessageCommand, DeleteMessageCommandDto>
    {
        public async Task<DeleteMessageCommandDto> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            if (client == null)
            {
                throw new BloomiaNotFoundException("Client not found");
            }
            var msg = await context.Messages.Include(x => x.DirectChat)
                .FirstOrDefaultAsync(x => x.Id == request.MessageId && x.DirectChatId == request.DirectChatId
                                    && x.DirectChat.ClientId == client.Id, cancellationToken);
            
            if(msg == null || msg.IsDeleted)
            {
                throw new BloomiaNotFoundException("Message not found!");
            }
            if(msg.SenderId!= client.Id)
            {
                throw new BloomiaNotFoundException("You can not delete this message!");
            }
            msg.IsDeleted = true;
            msg.Content = "This message was deleted!";
            await context.SaveChangesAsync(cancellationToken);

            var dto=new DeleteMessageCommandDto
            {
                 SenderType=msg.SenderType.ToString(),
                 Message = msg.Content,
                 IsDeleted = msg.IsDeleted
            };
            return dto;
        }
    }
}
