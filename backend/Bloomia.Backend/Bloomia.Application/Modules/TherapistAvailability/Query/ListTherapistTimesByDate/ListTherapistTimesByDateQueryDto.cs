using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Query.ListTherapistTimesByDate
{
    public class ListTherapistTimesByDateQueryDto
    {
        public DateOnly RequestedDate { get; set; }
        public List<ListTimesDto> AllSlotsOfDate { get; set; }=new List<ListTimesDto>();
    }
    public class ListTimesDto
    {
        public int TherapistAvailabilityId { get; set; }
        public TimeOnly StartTime { get; set; }
        public bool IsBooked { get; set; }
    }

}
