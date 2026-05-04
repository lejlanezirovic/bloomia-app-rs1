using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Query.List
{
    public sealed class ListTherapistAvailabilitiesForClientQuery : IRequest<ListAllTherapistAvailabilitiesQueryDto>
    {
        public int TherapistId { get; set; }
    }
}
