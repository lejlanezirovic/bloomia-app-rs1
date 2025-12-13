using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Delete.DeleteForTherapist
{
    public class DeleteMessageTherapistCommandHandler(IAppDbContext context) : IRequestHandler<DeleteMessageTherapistCommand, DeleteMessageTherapistCommandDto>
    {
        public async Task<DeleteMessageTherapistCommandDto> Handle(DeleteMessageTherapistCommand request, CancellationToken cancellationToken)
        {
            var therapist = await context.Therapists.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            if (therapist == null)
            {
                throw new BloomiaNotFoundException("Therapist not found");
            }

            var message = await context.Messages.Include(x => x.DirectChat)
                    .FirstOrDefaultAsync(x => x.Id == request.MessageId && x.DirectChatId == request.DirectChatId 
                            && x.DirectChat.TherapistId == therapist.Id, cancellationToken);

            if(message == null || message.IsDeleted)
            {
                throw new BloomiaNotFoundException("Message not found");
            }
            if (message.SenderId != therapist.Id)
            {
                throw new BloomiaConflictException("You can not delete this message!");
            }
            message.IsDeleted = true;
            message.Content="This message was deleted!.";
            await context.SaveChangesAsync(cancellationToken);
            var dto = new DeleteMessageTherapistCommandDto
            {
                Message = message.Content,
                SenderType = message.SenderType.ToString(),
                IsDeleted = message.IsDeleted
            };
            return dto;
        }
    }
}
