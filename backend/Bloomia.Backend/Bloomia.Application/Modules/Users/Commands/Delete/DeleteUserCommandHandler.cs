using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Users.Commands.Delete
{
    public class DeleteUserCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
    : IRequestHandler<DeleteUserCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken ct)
        {
            if (currentUser.UserId is null)
                throw new BloomiaBusinessRuleException("NOT_LOGGED_IN", "You have to be logged in.");

            //samo admin može obrisati korisnički račun 
            if (!currentUser.IsAdmin)
                throw new BloomiaBusinessRuleException("USER_NOT_AUTH", "Only admins can delete users.");


            var user = await context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Id == request.Id, ct);

            if (user is null)
                throw new BloomiaNotFoundException("User not found.");


            user.IsEnabled = false;

            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }


    }
}
