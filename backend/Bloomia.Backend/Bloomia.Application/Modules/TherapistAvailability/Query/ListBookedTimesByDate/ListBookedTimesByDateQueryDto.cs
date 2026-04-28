using Bloomia.Application.Modules.TherapistAvailability.Query.ListAllTimesByDate;
using Bloomia.Application.Modules.TherapistAvailability.Query.ListTherapistTimesByDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Query.ListBookedTimesByDate
{
    public class ListBookedTimesByDateQueryDto
    {
        public DateOnly RequestedDate { get; set; }
        public List<ListTimesDto> AllSlotsOfDate { get; set; } = new List<ListTimesDto>();
    }
}
