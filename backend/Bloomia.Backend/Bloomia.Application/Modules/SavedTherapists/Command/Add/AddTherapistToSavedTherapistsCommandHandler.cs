using Bloomia.Application.Modules.SavedTherapists.Queries.List;
using Bloomia.Domain.Entities;
using Bloomia.Domain.Entities.TherapistRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Command.Add
{
    public class AddTherapistToSavedTherapistsCommandHandler(IAppDbContext context) : IRequestHandler<AddTherapistToSavedTherapistsCommand, AddTherapistToSavedTherapistsCommandDto>
    {
        public async Task<AddTherapistToSavedTherapistsCommandDto> Handle(AddTherapistToSavedTherapistsCommand request, CancellationToken cancellationToken)
        {
            var client=await context.Clients.Include(x=>x.User).Where(x=>x.UserId==request.UserId).FirstOrDefaultAsync(cancellationToken);
            if(client==null)
            {
                throw new BloomiaNotFoundException("Klijent nije pronadjen!");
            }
            var therapist=await context.Therapists.Include(x=>x.User).Include(x=>x.MyTherapyTypesList).ThenInclude(x=>x.TherapyType)
                            .Where(x=>x.Id==request.TherapistId).FirstOrDefaultAsync(cancellationToken);
            if(therapist==null)
            {
                throw new BloomiaNotFoundException("Terapeut nije pronadjen!");
            }
            //provjeri postoji li u saved izvuci sve saved t za klijenta pa provjeri 
            var saved =await context.SavedTherapists.Include(x => x.Client).Include(x => x.Therapist)
                        .Where(x => x.ClientId == client.Id && x.TherapistId==therapist.Id).FirstOrDefaultAsync(cancellationToken);

            if(saved != null)
            {
                throw new BloomiaConflictException("Terapeut je vec sacuvan!");
            }
            var newSavedTherapist = new SavedTherapistsEntity
            {
                ClientId = client.Id,
                Client = client,
                TherapistId = therapist.Id,
                Therapist = therapist,
                SavedAt = DateTime.UtcNow
            };
            await context.SavedTherapists.AddAsync(newSavedTherapist, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
            
           //sad mi trebaju sve vrste terapija 

            var dto = new AddTherapistToSavedTherapistsCommandDto
            {
                TherapistId = therapist.Id,
                Fullname = therapist.User.Fullname,
                Specialization = therapist.Specialization,
                Description = therapist.Description,
                RatingAvg = therapist.RatingAvg
            };
            foreach(var item in therapist.MyTherapyTypesList)
            {
                var therapyTypeDto=new ListTherapistTherapyTypesQueryDto
                {
                    TherapistId = item.TherapistId,
                    TherapyTypeName = item.TherapyType.TherapyName
                };
                dto.MYTherapyTypes.Add(therapyTypeDto);
            }
            return dto;
        }
    }
}
