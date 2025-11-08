using Bloomia.Domain.Entities;
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
            //imam gender, location city i country 
            var gender = await db.Genders.FirstOrDefaultAsync(x => x.Name.ToLower() == request.GenderName.ToLower(), cancellationToken);
            var location = await db.Locations
                .FirstOrDefaultAsync(x => x.City.ToLower() == request.LocationCityName.ToLower() 
                    && x.Country.ToLower() == request.LocationCountryName.ToLower(), cancellationToken);
            var language=await db.Languages.FirstOrDefaultAsync(x=>x.Name.ToLower()==request.LanguageName.ToLower(), cancellationToken);
           
            if(gender ==null)
                throw new MarketConflictException("That gender doesn't exist in database");
            if (location ==null)
                throw new MarketConflictException($"Incorrect location {request.LocationCityName}- {request.LocationCountryName}");
            if(language==null)
                throw new MarketConflictException($"Language doesn't exist- {request.LanguageName}");

            var newUser = new UserEntity
            {
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Username = request.Username,
                Email = email,
                RoleId = clientRole.Id,
                Role = clientRole,
                GenderId=gender.Id,
                Gender=gender,
                LocationId=location.Id,
                Location = location,
                LanguageId=language.Id,
                Language=language,
                DateOfBirth=request.DateOfBirth,
                IsEnabled = true,
                Fullname =$"{request.Firstname} {request.Lastname}",
                CreatedAtUtc = DateTime.UtcNow
            };
            newUser.PasswordHash = hasher.HashPassword(newUser, request.Password);
            db.Users.Add(newUser);

            var existingClient=await db.Clients.FirstOrDefaultAsync(x=>x.UserId== newUser.Id, cancellationToken);
            if (existingClient != null)
                throw new Exception("Korisnik je vec registrovan kao klijent");

            var client = new ClientEntity {               
                UserId=newUser.Id,
                User=newUser,
                CreatedAtUtc=DateTime.UtcNow,
            };
            db.Clients.Add(client);
            await db.SaveChangesAsync(cancellationToken);

            newUser = await db.Users.Include(x => x.Role).FirstOrDefaultAsync(x=>x.Id==newUser.Id, cancellationToken);
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
                ExpiresAt=pair.RefreshTokenExpiresAtUtc,
                GenderName=newUser.Gender.Name,
                City=newUser.Location.City,
                Country=newUser.Location.Country,
                Language=newUser.Language.Name,
                DateOfBirth=newUser.DateOfBirth
            };
            return newUserDto;
        }
    }
}
