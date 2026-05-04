using Bloomia.Application.Modules.TherapistAvailability.Query.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bloomia.Application.Modules.TherapistAvailability.Query.ListTherapistTimesByDate
{
    public class ListTherapistTimesByDateQueryHandler(IAppDbContext context) : IRequestHandler<ListTherapistTimesByDateQuery, ListTherapistTimesByDateQueryDto>
    {
        public async Task<ListTherapistTimesByDateQueryDto> Handle(ListTherapistTimesByDateQuery request, CancellationToken cancellationToken)
        {
            var therapist = await context.Therapists.Include(x => x.User).Where(x => x.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                throw new BloomiaNotFoundException(message: "Therapist not found try to login first!");
            }
            
            var AllTimesByDate = await context.TherapistAvailabilities
                    .Where(x => x.TherapistId == therapist.Id && x.Date == request.Date &&
                            !x.IsDeleted).ToListAsync(cancellationToken);
            var dto = new ListTherapistTimesByDateQueryDto
            {
                RequestedDate = request.Date

            };
            foreach(var i in AllTimesByDate)
            {
                var dtoTime = new ListTimesDto
                {
                    TherapistAvailabilityId=i.Id,
                    StartTime = i.StartTime,
                    IsBooked = i.IsBooked
                };
                dto.AllSlotsOfDate.Add(dtoTime);
            }
            return dto;
        }
    }
}
