using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Command.Delete
{
    public sealed class DeleteAppointmentCommand:IRequest<string>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int AppointmentId { get; set; }
    }
}
