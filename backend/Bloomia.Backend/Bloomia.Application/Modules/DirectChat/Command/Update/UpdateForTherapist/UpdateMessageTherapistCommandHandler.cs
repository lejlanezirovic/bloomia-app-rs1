using Bloomia.Application.Modules.DirectChat.Command.Update.UpdateForClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Update.UpdateForTherapist
{
    public class UpdateMessageTherapistCommandHandler(IAppDbContext context,IChatNotifier chatNotifier) : IRequestHandler<UpdateMessageTherapistCommand, UpdateMessageTherapistCommandDto>
    {
        public async Task<UpdateMessageTherapistCommandDto> Handle(UpdateMessageTherapistCommand request, CancellationToken cancellationToken)
        {
            var therapist = await context.Therapists.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            if (therapist == null)
            {
                throw new BloomiaNotFoundException("Therapist not found");
            }
            var message= await context.Messages.Include(x => x.DirectChat).ThenInclude(x=>x.Client).ThenInclude(x=>x.User)
                .FirstOrDefaultAsync(x => x.Id == request.MessageId && x.DirectChatId == request.DirectChatId
                                    && x.DirectChat.TherapistId == therapist.Id, cancellationToken);

            if (message == null || message.IsDeleted)
            {
                throw new BloomiaNotFoundException("Message not found");
            }
            if (message.SenderId != therapist.Id)
            {
                throw new BloomiaConflictException("You can not update this message");
            }
            var dto = new UpdateMessageTherapistCommandDto
            {
                DirectChatId = message.DirectChatId.Value,
                MessageId = message.Id,
                OldMessage = message.Content,
                UpdatedContent = request.NewContent,
                IsRead = message.isRead
            };
            message.Content = request.NewContent;
            await context.SaveChangesAsync(cancellationToken);

            await chatNotifier.NotifyUserAsync(message.DirectChat.Client.User.Id.ToString(),
               "MessageUpdated", new
               {
                   DirectChatId = message.DirectChatId,
                   MessageId = message.Id,
                   IsDeleted = message.IsDeleted,
                   Content = message.Content
               }, cancellationToken
            );
            return dto;
        }
    }
}
