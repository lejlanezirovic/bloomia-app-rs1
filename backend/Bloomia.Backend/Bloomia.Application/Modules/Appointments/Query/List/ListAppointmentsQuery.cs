using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Query.List
{
    public sealed class ListAppointmentsQuery:IRequest<List<ListAppointmentsQueryDto>>
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
