using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Commands.Update.ChangeTherapistPassword
{
    public sealed class ChangeTherapistPasswordCommandHandler(IAppDbContext context, IPasswordHasher<UserEntity> hasher, IAppCurrentUser currentUser)
        : IRequestHandler<ChangeTherapistPasswordCommand, Unit>
    {
        public async Task<Unit> Handle(ChangeTherapistPasswordCommand request, CancellationToken ct)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsEnabled && !x.IsDeleted, ct);

            if (user is null)
                throw new BloomiaNotFoundException("User not found.");

            if (currentUser.IsTherapist && currentUser.UserId != request.Id)
                throw new BloomiaNotFoundException("Not authrized to change password.");

            if(currentUser.IsTherapist)
            {
                if (string.IsNullOrEmpty(request.CurrentPassword))
                    throw new BloomiaBusinessRuleException("", "You have to enter current password.");

                var verified = hasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);
                if (verified == PasswordVerificationResult.Failed)
                    throw new BloomiaBusinessRuleException("", "Current password is incorrect.");
            }

            user.PasswordHash = hasher.HashPassword(user, request.NewPassword);

            var nowUtc = DateTime.UtcNow;

            var activeTokens = await context.RefreshTokens
                .Where(x => x.UserId == user.Id && !x.IsRevoked)
                .ToListAsync(ct);

            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.RevokedAtUtc = nowUtc;
            }

            user.TokenVersion++;

            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
