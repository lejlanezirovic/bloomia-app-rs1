using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Queries.List
{
    public class ListSavedTherapistsQueryHandler(IAppDbContext context) : IRequestHandler<ListSavedTherapistsQuery, PageResult<ListSavedTherapistInfoDto>>
    {
        public async Task<PageResult<ListSavedTherapistInfoDto>> Handle(ListSavedTherapistsQuery request, CancellationToken cancellationToken)
        {
            // prvo naci klijenta na osnovu user id sto se posalje sa servera kojeg cemo naci preko claimova (name identifier)
            //onda cemo u bazu saved therapist porediti id klijenta da izvucemo spasene terapeute
            //onda  proci if casove 
            //mapirati u dto i vratiti paged result

            var client=await context.Clients.Include(x=>x.User).FirstOrDefaultAsync(x=>x.UserId==request.UserId,cancellationToken);
            if (client ==null)
            {
                throw new BloomiaNotFoundException("Klijent nije pronadjen morate se logirati ili registrovati!");
            }
            var query = context.SavedTherapists.Where(x => x.ClientId == client.Id)
                          .Select(x => new ListSavedTherapistInfoDto
                          {
                              TherapistId = x.Therapist.Id,
                              Fullname = x.Therapist.User.Fullname,
                              Specialization = x.Therapist.Specialization,
                              Description = x.Therapist.Description,
                              RatingAvg = x.Therapist.RatingAvg,

                              MYTherapyTypes = x.Therapist.MyTherapyTypesList
                                      .Select(t => new ListTherapistTherapyTypesQueryDto
                                      {
                                          TherapistId = t.TherapistId,
                                          TherapyTypeName = t.TherapyType.TherapyName
                                      }).ToList()
                          }).AsNoTracking();

            return await PageResult<ListSavedTherapistInfoDto>.FromQueryableAsync(query, request.Paging, cancellationToken);

        }
    }
}
