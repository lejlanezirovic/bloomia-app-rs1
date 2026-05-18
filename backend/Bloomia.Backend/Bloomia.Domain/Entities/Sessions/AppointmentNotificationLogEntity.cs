using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Common;
using Bloomia.Domain.Entities.Enums;

namespace Bloomia.Domain.Entities.Sessions
{
    public class AppointmentNotificationLogEntity : BaseEntity
    {
        public int AppointmentId { get; set; }
        public AppointmentEntity Appointment { get; set; } = default!;
        public string RecipientEmail { get; set; } = string.Empty;
        public AppointmentNotificationsType NotificationType { get; set; }
        public AppointmentNotificationStatus Status { get; set; }
        public DateTime? SentAtUtc { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
