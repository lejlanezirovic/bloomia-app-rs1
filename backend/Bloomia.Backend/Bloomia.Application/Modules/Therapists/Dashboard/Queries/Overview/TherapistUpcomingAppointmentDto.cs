using Bloomia.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Queries.Overview
{
    public class TherapistUpcomingAppointmentDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime ScheduledAtUtc { get; set; }
        public SessionType SessionType { get; set; }
    }
}
