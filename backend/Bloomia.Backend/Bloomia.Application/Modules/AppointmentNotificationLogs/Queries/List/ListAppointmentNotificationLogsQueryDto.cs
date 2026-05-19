using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.AppointmentNotificationLogs.Queries.List
{
    public class ListAppointmentNotificationLogsQueryDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string NotificationType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? SentAtUtc { get; set; }
        public string? ErrorMessage { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string TherapistName { get; set; } = string.Empty;
        public DateTime ScheduledAtUtc { get; set; }
        public string SessionType { get; set; }  = string.Empty;
    }
}
