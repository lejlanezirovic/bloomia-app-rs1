using Bloomia.Application.Modules.SavedTherapists.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Queries.GetById
{
    public class GetSavedTherapistByIdCommandHandler(IAppDbContext context) : IRequestHandler<GetSavedTherapistByIdCommand, GetSavedTherapistByIdCommandDto>
    {
        public async Task<GetSavedTherapistByIdCommandDto> Handle(GetSavedTherapistByIdCommand request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.Include(x => x.User).AsNoTracking()
                    .Where(x => x.User.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (client == null)
            {
                throw new BloomiaNotFoundException("Klijent nije pronadjen.");
            }
            var therapist = await context.Therapists.Include(x => x.User).AsNoTracking()
                    .Where(x => x.Id == request.TherapistId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                throw new BloomiaNotFoundException("Terapeut nije pronadjen.");
            }

            var clientSavedTherapist = await context.SavedTherapists.Include(x => x.Therapist).ThenInclude(x => x.User)
                    .Include(x => x.Therapist).ThenInclude(x=>x.MyTherapyTypesList).ThenInclude(x=>x.TherapyType).Include(x => x.Client)
                     .AsNoTrackingWithIdentityResolution().Where(x => x.Client.Id == client.Id && x.Therapist.Id == therapist.Id).FirstOrDefaultAsync(cancellationToken);
           
            if(clientSavedTherapist==null || clientSavedTherapist.IsDeleted)
            {
                throw new BloomiaNotFoundException("Therapeut se ne nalazi u vasoj listi sacuvanih terapeuta.");
            }

            var savedTherapistDto = new GetSavedTherapistByIdCommandDto
            {
                TherapistId = clientSavedTherapist.Therapist.Id,
                TherapistProfilePictureUrl = clientSavedTherapist.Therapist.User.ProfileImage,
                FullName = clientSavedTherapist.Therapist.User.Fullname,
                Specialization = clientSavedTherapist.Therapist.Specialization,
                Description = clientSavedTherapist.Therapist.Description,
                RatingAvg = clientSavedTherapist.Therapist.RatingAvg
            };

            foreach(var t in clientSavedTherapist.Therapist.MyTherapyTypesList)
            {
                var therapyTypeDto = new ListTherapistTherapyTypesQueryDto
                {
                    TherapyTypeName = t.TherapyType.TherapyName
                };
                savedTherapistDto.MyTherapyTypes.Add(therapyTypeDto);
            }
            /*•	Consider projecting to a DTO (Select(...)) instead of loading entities at all — 
             * best performance and avoids tracking entirely.*/
            return savedTherapistDto;
        }
    }
}
