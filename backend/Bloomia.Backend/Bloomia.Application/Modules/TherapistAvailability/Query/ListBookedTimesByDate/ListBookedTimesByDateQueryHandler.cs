using Bloomia.Application.Modules.TherapistAvailability.Query.ListAllTimesByDate;
using Bloomia.Application.Modules.TherapistAvailability.Query.ListTherapistTimesByDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Query.ListBookedTimesByDate
{
    public class ListBookedTimesByDateQueryHandler(IAppDbContext context) : IRequestHandler<ListBookedTimesByDateQuery, ListBookedTimesByDateQueryDto>
    {
        public async Task<ListBookedTimesByDateQueryDto> Handle(ListBookedTimesByDateQuery request, CancellationToken cancellationToken)
        {
            var therapist = await context.Therapists.Include(x => x.User).Where(x => x.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                throw new BloomiaNotFoundException(message: "Therapist not found try to login first!");
            }

            var AllBookedTimes = await context.TherapistAvailabilities
                    .Where(x => x.TherapistId == therapist.Id && x.Date == request.Date &&
                            !x.IsDeleted && x.IsBooked).ToListAsync(cancellationToken);
            var dto = new ListBookedTimesByDateQueryDto
            {
                RequestedDate = request.Date

            };
            foreach (var i in AllBookedTimes)
            {
                var dtoTime = new ListTimesDto
                {
                    StartTime = i.StartTime,
                    IsBooked = i.IsBooked
                };
                dto.AllSlotsOfDate.Add(dtoTime);
            }
            return dto;
        }
    }
}
