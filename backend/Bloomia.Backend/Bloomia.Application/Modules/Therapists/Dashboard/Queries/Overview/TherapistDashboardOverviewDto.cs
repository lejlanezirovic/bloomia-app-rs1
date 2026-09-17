using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Queries.Overview
{
    public class TherapistDashboardOverviewDto
    {
        public int AppointmentsThisMonth { get; set; }
        public int PastAppointmentsThisMonth { get; set; }
        public int ActiveClients { get; set; }
        public float AverageRating { get; set; }
        public List<TherapistUpcomingAppointmentDto> UpcomingAppointments { get; set; } = [];
    }
}
