using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.Sessions;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Create.CreateForClient
{
    public class SendMessageCommandHandler(IAppDbContext context, IChatNotifier chatNotifier) : IRequestHandler<SendMessageCommand, SendMessageCommandDto>
    {
        public async Task<SendMessageCommandDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            var therapist=await context.Therapists.FirstOrDefaultAsync(x=>x.Id==request.TherapistId,cancellationToken);
            if (client == null || therapist == null)
            {
                throw new Exception("Client or Therapist not found");
            }
            //provjera postoji li DIREKTAN chat izmedju njih
            var directChat = await context.DirectChats.Include(x => x.Client)
                      .Include(x => x.Therapist).ThenInclude(x=>x.User).Include(x => x.Messages)
                      .FirstOrDefaultAsync(x => x.TherapistId == therapist.Id && x.ClientId == client.Id, cancellationToken);
            if (directChat==null)
            { 
                directChat = new DirectChatEntity
                {
                    ClientId = client.Id,
                    Client = client,
                    TherapistId = therapist.Id,
                    Therapist = therapist
                };
                context.DirectChats.Add(directChat);
                await context.SaveChangesAsync(cancellationToken);
            }
            var message = new MessageEntity
            {
                DirectChatId = directChat.Id,
                DirectChat = directChat,
                Content = request.Content,
                SenderId = client.Id,
                SenderType = SenderType.CLIENT,
                CreatedAtUtc = DateTime.UtcNow,
                isRead = false,
                SentAt = DateTime.UtcNow
            };
            context.Messages.Add(message);
            await context.SaveChangesAsync(cancellationToken);

            var dto = new SendMessageCommandDto
            {
                Note = "Sent!",
                Message = message.Content,
                SentAt = message.SentAt,
                DirectChatId= directChat.Id,
                MessageId=message.Id
            };
            await chatNotifier.NotifyUserAsync(therapist.UserId.ToString(), "ReceiveMessage",
                 new
                 {
                     DirectChatId = directChat.Id,
                     MessageId = message.Id,
                     SenderId = client.Id,
                     SenderType = "CLIENT",
                     Content=message.Content,
                     SentAt= message.SentAt,
                     IsRead=message.isRead
                 }, cancellationToken
            );
            
            return dto;

            ////Klijent salje poruku terapeutu i ovim NotifiyUserAsync() zelimo poslati obavjestenje terapeutu da je dobio poruku
        }
    }
}
