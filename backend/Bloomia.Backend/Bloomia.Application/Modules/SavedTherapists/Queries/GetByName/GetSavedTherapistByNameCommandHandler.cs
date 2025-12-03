using Bloomia.Application.Modules.SavedTherapists.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Queries.GetByName
{
    public class GetSavedTherapistByNameCommandHandler(IAppDbContext context) : IRequestHandler<GetSavedTherapistByNameCommand, List<GetSavedTherapistByNameCommandDto>>
    {
        public async Task<List<GetSavedTherapistByNameCommandDto>> Handle(GetSavedTherapistByNameCommand request, CancellationToken cancellationToken)
        {
            //trazimo za klijenta u listi spasenih terapeuta po imenu
            var filter=(request.SerachName?? string.Empty).Trim().ToLower();

            var client = await context.Clients.Include(x => x.User).AsNoTracking()
                               .Where(x => x.User.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

            if (client == null)
            {
                throw new BloomiaNotFoundException("Klijent nije pronadjen.");
            }

            if (!string.IsNullOrWhiteSpace(filter)) {
                var savedTherapist = await context.SavedTherapists.AsNoTracking()
               .Include(x => x.Therapist).ThenInclude(x => x.User)
               .Include(x => x.Therapist).ThenInclude(x => x.MyTherapyTypesList).ThenInclude(x => x.TherapyType)
               .Include(x => x.Client).Where(x => x.ClientId == client.Id && (x.Therapist.User.Firstname.ToLower()
               .Contains(filter) || x.Therapist.User.Lastname.ToLower().Contains(filter)))
               .Select(x => new GetSavedTherapistByNameCommandDto
               {
                   TherapistId = x.TherapistId,
                   FullName = x.Therapist.User.Fullname,
                   TherapistProfilePictureUrl = x.Therapist.User.ProfileImage,
                   Specialization = x.Therapist.Specialization,
                   Description = x.Therapist.Description,
                   RatingAvg = x.Therapist.RatingAvg,

                   MyTherapyTypes = x.Therapist.MyTherapyTypesList.Select(t => new ListTherapistTherapyTypesQueryDto
                   {
                       TherapyTypeName = t.TherapyType.TherapyName
                   }).ToList()

               }).ToListAsync(cancellationToken);

                if (!savedTherapist.Any())
                {
                    throw new BloomiaNotFoundException("Nije pronadjen terapeut sa tim imenom u vasoj listi sacuvanih");
                }
                return savedTherapist;
            }
            else
            { 
                var allSavedTherapists = await context.SavedTherapists.AsNoTracking()
                .Include(x => x.Therapist).ThenInclude(x => x.User)
                .Include(x => x.Therapist).ThenInclude(x => x.MyTherapyTypesList).ThenInclude(x => x.TherapyType)
                .Include(x => x.Client).Where(x => x.ClientId == client.Id)
                .Select(x => new GetSavedTherapistByNameCommandDto
                {
                    TherapistId = x.TherapistId,
                    FullName = x.Therapist.User.Fullname,
                    TherapistProfilePictureUrl = x.Therapist.User.ProfileImage,
                    Specialization = x.Therapist.Specialization,
                    Description = x.Therapist.Description,
                    RatingAvg = x.Therapist.RatingAvg,

                    MyTherapyTypes = x.Therapist.MyTherapyTypesList.Select(t => new ListTherapistTherapyTypesQueryDto
                    {
                        TherapyTypeName = t.TherapyType.TherapyName
                    }).ToList()

                }).ToListAsync(cancellationToken);
               return allSavedTherapists;

            }
               
        }
    }
}
