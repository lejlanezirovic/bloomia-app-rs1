using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Entities.TherapistRelated;
using Microsoft.AspNetCore.Identity;

namespace Bloomia.Application.Modules.Therapists.Commands.Update
{
    public sealed class UpdateTherapistCommandHandler(IAppDbContext context, IAppCurrentUser currentUser, IPasswordHasher<UserEntity> hasher)
        : IRequestHandler<UpdateTherapistCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateTherapistCommand request, CancellationToken ct)
        {
            var therapist = await context.Therapists
                .Include(x => x.User)
                .Include(x => x.MyTherapyTypesList)
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(ct);

            if (therapist is null)
            {
                throw new BloomiaNotFoundException($"Terapeut s ID-jem {request.Id} nije pronađen.");
            }

            if (!currentUser.IsAdmin && therapist.UserId != currentUser.UserId)
                throw new BloomiaBusinessRuleException("USER_NOT_AUTH", "Only admins and account owner can make updates.");

            if(!string.IsNullOrEmpty(request.Specialization))
                therapist.Specialization = request.Specialization;

            if(!string.IsNullOrEmpty(request.Description))
                therapist.Description = request.Description;

            if(request.TherapyTypeIds != null)
            {
                var existingIds = therapist.MyTherapyTypesList
                    .Select(x => x.TherapyTypeId)
                    .ToList();

                var newIds = request.TherapyTypeIds;

                var therapyTypesToRemove = therapist.MyTherapyTypesList
                    .Where(x => !newIds.Contains(x.TherapyTypeId))
                    .ToList();

                foreach(var item in therapyTypesToRemove)
                    context.TherapistsTherapyTypes.Remove(item);

                //dodavanje nove terapije
                var therapyTypesToAdd = newIds.Except(existingIds).ToList();

                foreach(var id in therapyTypesToAdd)
                {
                    therapist.MyTherapyTypesList.Add(new TherapistsTherapyTypesEntity
                    {
                        TherapistId = therapist.Id,
                        TherapyTypeId = id
                    });
                }
            }

            //update user entity polja unutar therapista
            var user = therapist.User;

            if(!string.IsNullOrEmpty(request.Email))
            {
                var emailExists = await context.Users
                    .AnyAsync(x => x.Email == request.Email && x.Id != user.Id, ct);

                if (emailExists)
                    throw new BloomiaConflictException("Email address not available.");
                
                user.Email = request.Email;
            }

            if(!string.IsNullOrEmpty(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;

            if(request.LocationId.HasValue)
            {
                var locationExists = await context.Locations
                    .AnyAsync(x => x.Id == request.LocationId.Value, ct);

                if (!locationExists)
                    throw new BloomiaNotFoundException("Location not valid.");

                user.LocationId = request.LocationId.Value;
            }


            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
