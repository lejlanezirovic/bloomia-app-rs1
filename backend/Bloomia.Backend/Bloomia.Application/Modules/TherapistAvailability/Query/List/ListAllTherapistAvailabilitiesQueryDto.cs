
using Bloomia.Application.Modules.TherapistAvailability.Query.ListTherapistTimesByDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Query.List
{
    public class ListAllTherapistAvailabilitiesQueryDto
    {
        public int TherapistId { get; set; }
        public List<ListDateAndSlotsDto> WorkingDates { get; set; }= new List<ListDateAndSlotsDto>();
    }
    public class ListDateAndSlotsDto
    {
        public DateOnly Date { get; set; }
        public List<ListTimesDto> AllSlotsOfDate { get; set; } = new List<ListTimesDto>();
    }
}
