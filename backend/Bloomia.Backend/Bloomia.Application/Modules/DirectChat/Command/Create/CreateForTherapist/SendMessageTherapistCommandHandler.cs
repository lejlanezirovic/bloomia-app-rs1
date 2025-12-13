using Bloomia.Application.Modules.DirectChat.Command.Create;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.Sessions;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Create.CreateForTherapist
{
    public class SendMessageTherapistCommandHandler(IAppDbContext context, IHubContext<ChatHub> hubContext) : IRequestHandler<SendMessageTherapistCommand, SendMessageTherapistCommandDto>
    {
        public async Task<SendMessageTherapistCommandDto> Handle(SendMessageTherapistCommand request, CancellationToken cancellationToken)
        {
            var therapist = await context.Therapists.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
           
            var client = await context.Clients.FirstOrDefaultAsync(x =>x.Id==request.ClientId, cancellationToken);

            if (client == null || therapist == null)
            {
                throw new Exception("Client or Therapist not found");
            }
            //provjera postoji li DIREKTAN chat izmedju njih
            var directChat = await context.DirectChats.Include(x => x.Client).ThenInclude(x=>x.User).Include(x => x.Therapist)
                      .Include(x => x.Messages)
                      .FirstOrDefaultAsync(x => x.TherapistId == therapist.Id && x.ClientId == client.Id, cancellationToken);
            if (directChat == null)
            {
                directChat = new DirectChatEntity
                {
                    TherapistId = therapist.Id,
                    Therapist = therapist,
                    ClientId = client.Id,
                    Client = client
                };
                context.DirectChats.Add(directChat);
                await context.SaveChangesAsync(cancellationToken);
            }
            var message = new MessageEntity
            {
                DirectChatId = directChat.Id,
                DirectChat = directChat,
                Content = request.Content,
                SenderId = therapist.Id,
                SenderType = SenderType.TTHERAPIST,
                CreatedAtUtc = DateTime.UtcNow,
                isRead = false,
                SentAt = DateTime.UtcNow
            };
            context.Messages.Add(message);
            await context.SaveChangesAsync(cancellationToken);

            var dto = new SendMessageTherapistCommandDto
            {
                Note = "Sent!",
                Message = message.Content,
                SentAt = message.SentAt
            };
            await hubContext.Clients.User(client.User.Id.ToString())
                .SendAsync("ReceiveDirectMessage", new
                {
                    DirectChatId = directChat.Id,
                    Sender = "Therapist",
                    Message = message.Content,
                    message.SentAt
                }, cancellationToken);
            return dto;
        }
    }
    
}
