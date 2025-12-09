using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Query.List
{
    public class ListAppointmentsQueryDto
    {
        public int AppointmentId { get; set; }
        public string TherapistName { get; set; }
        public string ClientName { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string SessionType { get; set; }
        public string Status { get; set; }
        // public List<ListAppointments> ClientAppointments { get; set; } = new List<ListAppointments>();
    }
}
