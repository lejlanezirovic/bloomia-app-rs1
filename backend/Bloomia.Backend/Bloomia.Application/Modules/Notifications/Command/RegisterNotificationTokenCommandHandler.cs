using Bloomia.Domain.Entities.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Notifications.Command
{
    public class RegisterNotificationTokenCommandHandler(IAppDbContext context) : IRequestHandler<RegisterNotificationTokenCommand, Unit>
    {
        public async Task<Unit> Handle(RegisterNotificationTokenCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var token= request.Token.Trim();
            if (string.IsNullOrEmpty(token))
            {
                throw new BloomiaBusinessRuleException("token","Obavezan token");
            }
            var tokenForUser = await context.NotificationTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.Token == token, cancellationToken);

            if(tokenForUser != null)
            {
                tokenForUser.IsActive = true;
                return Unit.Value;
            }
            //u slucaju da ne postoji sacuvati
            //za ovog usera sacuvaj mi ovaj token novi entitet dodati u bazu
            var newTokenForUser = new NotificationTokenEntity
            {
                UserId = userId,
                Token = request.Token,
                CreatedAt = DateTime.UtcNow,
                IsActive = true

            };
            context.NotificationTokens.Add(newTokenForUser);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
