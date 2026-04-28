using Bloomia.Application.Modules.TherapistAvailability.Query.ListAllTimesByDate;
using Bloomia.Application.Modules.TherapistAvailability.Query.ListTherapistTimesByDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Query.List
{
    public class ListAllTherapistsAvailabilitiesQueryHandler(IAppDbContext context) : IRequestHandler<ListAllTherapistAvailabilitiesQuery, ListAllTherapistAvailabilitiesQueryDto>
    {
        public async Task<ListAllTherapistAvailabilitiesQueryDto> Handle(ListAllTherapistAvailabilitiesQuery request, CancellationToken cancellationToken)
        {
            var therapist = await context.Therapists.Include(x => x.User).Where(x => x.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                throw new BloomiaNotFoundException(message: "Therapist not found try to login first!");
            }
            var availabilities =context.TherapistAvailabilities.Where(x => x.TherapistId == therapist.Id && !x.IsDeleted).AsNoTracking();

            if (availabilities.Count() == 0)
            {
                throw new BloomiaNotFoundException("Nije pronadjen niti jedan zakazan termin!");
            }
            var AvailabilityDto = new ListAllTherapistAvailabilitiesQueryDto
            {
                TherapistId=therapist.Id,
                WorkingDates=availabilities.GroupBy(x=>x.Date).Select(g=> new ListDateAndSlotsDto
                {
                    Date=g.Key,
                    AllSlotsOfDate=g.Select(x=> new ListTimesDto
                    {
                        TherapyAvailabilityId=x.Id,
                        StartTime=x.StartTime,
                        IsBooked=x.IsBooked
                    }).OrderBy(x=>x.StartTime).ToList()
                }).OrderBy(x=>x.Date).ToList()
            };
            return AvailabilityDto;
        }
    }
}
