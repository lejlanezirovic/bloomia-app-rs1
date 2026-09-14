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

            if (!string.IsNullOrWhiteSpace(request.FullName))
                query = query.Where(x => x.Fullname.ToLower().Contains(request.FullName.ToLower()));

            if (!string.IsNullOrWhiteSpace(request.Specialization))
                query = query.Where(x => x.Specialization.ToLower().Contains(request.Specialization.ToLower()));

            if (request.MinRating.HasValue)
                query = query.Where(x => x.RatingAvg >= request.MinRating.Value);

            if (!string.IsNullOrWhiteSpace(request.TherapyType))
                query = query.Where(x => x.MYTherapyTypes.Any(t => t.TherapyTypeName.ToLower().Contains(request.TherapyType.ToLower())));

            query = request.SortByRatingDesc
                ? query.OrderByDescending(x => x.RatingAvg)
                : query;

            return await PageResult<ListSavedTherapistInfoDto>.FromQueryableAsync(query, request.Paging, cancellationToken);

        }
    }
}
