using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Modules.TherapistAvailability.Query.ListTherapistTimesByDate;

namespace Bloomia.Application.Modules.TherapistAvailability.Query.List
{
    public sealed class ListTherapistAvailabilitiesForClientQueryHandler(IAppDbContext context) : IRequestHandler<ListTherapistAvailabilitiesForClientQuery, ListAllTherapistAvailabilitiesQueryDto>
    {
        public async Task<ListAllTherapistAvailabilitiesQueryDto> Handle(ListTherapistAvailabilitiesForClientQuery request, CancellationToken ct)
        {
            var therapistExists = await context.Therapists
                .AnyAsync(x => x.Id == request.TherapistId && !x.IsDeleted, ct);

            if (!therapistExists)
                throw new BloomiaNotFoundException("Therapist not found.");

            var bihTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowUtc = DateTime.UtcNow;

            var rawAvailabilities = await context.TherapistAvailabilities
                .Where(x => x.TherapistId == request.TherapistId && !x.IsDeleted)
                .OrderBy(x => x.Date)
                .ThenBy(x => x.StartTime)
                .Select(x => new
                {
                    x.Date,
                    x.Id,
                    x.StartTime,
                    x.IsBooked
                }).ToListAsync(ct);

            var futureAvailabilities = rawAvailabilities
                .Where(x =>
                {
                    var localDateTime = x.Date.ToDateTime(x.StartTime);
                    var slotUtc = TimeZoneInfo.ConvertTimeToUtc(localDateTime, bihTimeZone);
                    return slotUtc > nowUtc;
                }).ToList();

            var dto = new ListAllTherapistAvailabilitiesQueryDto
            {
                TherapistId = request.TherapistId,
                WorkingDates = futureAvailabilities
                    .GroupBy(x => x.Date)
                    .Select(g => new ListDateAndSlotsDto
                    {
                        Date = g.Key,
                        AllSlotsOfDate = g.Select(x => new ListTimesDto
                        {
                            TherapyAvailabilityId = x.Id,
                            StartTime = x.StartTime,
                            IsBooked = x.IsBooked,
                        }).ToList()
                    }).ToList()
            };

            return dto;
        }
    }
}
