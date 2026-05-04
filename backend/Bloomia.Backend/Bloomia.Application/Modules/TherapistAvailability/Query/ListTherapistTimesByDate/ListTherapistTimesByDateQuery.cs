using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Modules.TherapistAvailability.Query.ListTherapistTimesByDate;

namespace Bloomia.Application.Modules.TherapistAvailability.Query.ListTherapistTimesByDate
{
    public class ListTherapistTimesByDateQuery:IRequest<ListTherapistTimesByDateQueryDto>
    {
        public DateOnly Date { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
