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

            var bihTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowUtc = DateTime.UtcNow;

            var rawAvailabilities = await context.TherapistAvailabilities
                .Where(x => x.TherapistId == therapist.Id && !x.IsDeleted)
                .OrderBy(x => x.Date)
                .ThenBy(x => x.StartTime)
                .Select(x => new
                {
                    x.Date,
                    x.Id,
                    x.StartTime,
                    x.IsBooked
                }).ToListAsync(cancellationToken);

            var futureAvailabilities = rawAvailabilities
                .Where(x =>
                {
                    var localDateTime = x.Date.ToDateTime(x.StartTime);
                    var slotUtc = TimeZoneInfo.ConvertTimeToUtc(localDateTime, bihTimeZone);
                    return slotUtc > nowUtc;
                }).ToList();

            var availabilityDto = new ListAllTherapistAvailabilitiesQueryDto
            {
                TherapistId = therapist.Id,
                WorkingDates = futureAvailabilities
                    .GroupBy(x => x.Date)
                    .Select(g => new ListDateAndSlotsDto
                    {
                        Date = g.Key,
                        AllSlotsOfDate = g
                            .Select(x => new ListTimesDto
                            {
                                TherapistAvailabilityId = x.Id,
                                StartTime = x.StartTime,
                                IsBooked = x.IsBooked
                            })
                            .OrderBy(x => x.StartTime)
                            .ToList()
                    }).OrderBy(x => x.Date).ToList()
            };

            return availabilityDto;
        }
    }
}
