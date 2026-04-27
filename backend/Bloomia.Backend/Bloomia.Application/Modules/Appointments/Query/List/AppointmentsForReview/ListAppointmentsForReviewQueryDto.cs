using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Query.List.AppointmentsForReview
{
    public sealed class ListAppointmentsForReviewQueryDto
    {
        public int AppointmentId { get; set; }
        public DateTime ScheduledAtUtc { get; set; }
    }
}
