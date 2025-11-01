using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Auth.Commands.Register
{
    public sealed class UserRegisterCommandHandler(IAppDbContext db, IJwtTokenService jwt, IPasswordHasher<UserEntity> hasher)
        : IRequestHandler<UserRegisterCommand, UserRegisterCommandDto>
    {
        public async Task<UserRegisterCommandDto> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLower();
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email &&
                        x.IsEnabled == true && !x.IsDeleted, cancellationToken);

            if(user != null)
            { 
                throw new MarketConflictException("Korisnik sa unesenim emailom već postoji.");
            }

            var clientRole =await db.Roles.FirstOrDefaultAsync(x => x.RoleName == "CLIENT", cancellationToken);
            if (clientRole == null)
                throw new MarketConflictException("Rola ne postoji u bazi");
           
            var newUser = new UserEntity
            {
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Username = request.Username,
                Email = email,
                RoleId = clientRole.Id,
                Role = clientRole,
                IsEnabled = true,
                Fullname =$"{request.Firstname} {request.Lastname}",
                CreatedAtUtc = DateTime.UtcNow
            };
            newUser.PasswordHash = hasher.HashPassword(newUser, request.Password);
            db.Users.Add(newUser);
            await db.SaveChangesAsync(cancellationToken);

            newUser= await db.Users.Include(x => x.Role).FirstOrDefaultAsync(x=>x.Id==newUser.Id, cancellationToken);
            var pair=jwt.IssueTokens(newUser);

            var refreshTokenEnt = new RefreshTokenEntity
            { 
                UserId = newUser.Id,
                User=newUser,
                TokenHash=pair.RefreshTokenHash,
                ExpiresAtUtc=pair.RefreshTokenExpiresAtUtc,

            };
            db.RefreshTokens.Add(refreshTokenEnt);
            await db.SaveChangesAsync(cancellationToken);

            var newUserDto = new UserRegisterCommandDto
            {
                Id = newUser.Id,    
                Email = newUser.Email,
                AccessToken=pair.AccessToken,
                RefreshToken=pair.RefreshTokenRaw,
                Fullname=newUser.Fullname,
                RoleName=newUser.Role.RoleName,
                ExpiresAt=pair.RefreshTokenExpiresAtUtc
            };
            return newUserDto;
        }
    }
}
