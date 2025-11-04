using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bloomia.Application.Modules.Auth.Commands.Register.Therapist
{
    public sealed class TherapistRegisterCommandHandler(IAppDbContext context, IJwtTokenService jwt, IPasswordHasher<UserEntity> hasher)
        : IRequestHandler<TherapistRegisterCommand, TherapistRegisterCommandDto>
    {
        public async Task<TherapistRegisterCommandDto> Handle(TherapistRegisterCommand request, CancellationToken ct)
        {
            var email = request.Email.Trim().ToLower();

            //provjera da li već postoji korisnik sa tim emailom
            var existingUser = await context.Users
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email && x.IsEnabled && !x.IsDeleted, ct);

            if (existingUser != null)
                throw new BloomiaConflictException("Korisnik sa unesenim emailom već postoji.");

            //dohvatamo rolu THERAPIST
            var therapistRole = await context.Roles
                .FirstOrDefaultAsync(x => x.RoleName == "THERAPIST", ct);
            if (therapistRole == null)
                throw new BloomiaNotFoundException("Rola 'THERAPIST' ne postoji u bazi.");

            //kreiranje UserEntity-ja
            var newUser = new UserEntity
            {
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Fullname = $"{request.Firstname} {request.Lastname}",
                Username = request.Username,
                Email = email,
                RoleId = therapistRole.Id,
                Role = therapistRole,
                IsEnabled = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            newUser.PasswordHash = hasher.HashPassword(newUser, request.Password);
            context.Users.Add(newUser);
            await context.SaveChangesAsync(ct);

            //kreiranje TherapistEntity-ja
            var newTherapist = new TherapistEntity
            {
                UserId = newUser.Id,
                Specialization = request.Specialization,
                Description = request.Description,
                DocumentId = request.DocumentId,
                RatingAvg = 0,
                isVerified = false,
                CreatedAtUtc = DateTime.UtcNow
            };

            context.Therapists.Add(newTherapist);
            await context.SaveChangesAsync(ct);

            //generisanje JWT tokena
            newUser = await context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == newUser.Id, ct);

            var tokenPair = jwt.IssueTokens(newUser);

            var refreshToken = new RefreshTokenEntity
            {
                UserId = newUser.Id,
                TokenHash = tokenPair.RefreshTokenHash,
                ExpiresAtUtc = tokenPair.RefreshTokenExpiresAtUtc
            };

            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync(ct);

            var therapistDto = new TherapistRegisterCommandDto
            {
                Id = newUser.Id,
                Fullname = newUser.Fullname,
                Email = newUser.Email,
                RoleName = newUser.Role.RoleName,
                AccessToken = tokenPair.AccessToken,
                RefreshToken = tokenPair.RefreshTokenRaw,
                ExpiresAt = tokenPair.RefreshTokenExpiresAtUtc
            };

            return therapistDto;
        }
    }
}
