using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Command.Create
{
    public class CreateAppointmentCommandDto
    {
        public string Note { get; set; }
        public string TherapistFullname { get; set; }
        public DateTime BookedAt { get; set; }
        public string SessionType { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
    }
}
