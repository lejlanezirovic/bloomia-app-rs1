using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Entities.Admin;

namespace Bloomia.Application.Modules.Auth.Commands.Register.Admin
{
    public sealed class AdminRegisterCommandHandler(IAppDbContext context, IJwtTokenService jwt, IPasswordHasher<UserEntity> hasher)
        : IRequestHandler<AdminRegisterCommand, AdminRegisterCommandDto>
    {
        public async Task<AdminRegisterCommandDto> Handle(AdminRegisterCommand request, CancellationToken ct)
        {
            var email = request.Email.Trim().ToLower();

            // Check whether a user with the same email already exists.
            var existingUser = await context.Users
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email && x.IsEnabled && !x.IsDeleted, ct);

            if (existingUser != null)
                throw new BloomiaConflictException("A user with the entered email already exists.");

            // Get the ADMIN role.
            var adminRole = await context.Roles
                .FirstOrDefaultAsync(x => x.RoleName == "ADMIN", ct);
            if (adminRole == null)
                throw new BloomiaNotFoundException("Role 'ADMIN' does not exist in the database.");

            //create the UserEntity
            var newUser = new UserEntity
            {
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Fullname = $"{request.Firstname} {request.Lastname}",
                Email = email,
                Username = request.Username,
                RoleId = adminRole.Id,
                Role = adminRole,
                IsEnabled = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            newUser.PasswordHash = hasher.HashPassword(newUser, request.Password);
            context.Users.Add(newUser);
            await context.SaveChangesAsync(ct);

            //create the AdminEntity
            var newAdmin = new AdminEntity
            {
                UserId = newUser.Id,
                CreatedAtUtc = DateTime.UtcNow
            };

            context.Admins.Add(newAdmin);
            await context.SaveChangesAsync(ct);

            //generisanje JWT tokena
            newUser = await context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == newUser.Id, ct);
            
            if (newUser == null)
                throw new BloomiaNotFoundException("User was not found.");

            var tokenPair = jwt.IssueTokens(newUser);

            var refreshToken = new RefreshTokenEntity
            {
                UserId = newUser.Id,
                TokenHash = tokenPair.RefreshTokenHash,
                ExpiresAtUtc = tokenPair.RefreshTokenExpiresAtUtc
            };

            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync(ct);

            var adminDto = new AdminRegisterCommandDto
            {
                Id = newUser.Id,
                Fullname = newUser.Fullname ?? string.Empty,
                Email = newUser.Email,
                RoleName = newUser.Role.RoleName,
                AccessToken = tokenPair.AccessToken,
                RefreshToken = tokenPair.RefreshTokenRaw,
                ExpiresAt = tokenPair.RefreshTokenExpiresAtUtc
            };

            return adminDto;
        }
    }
}
