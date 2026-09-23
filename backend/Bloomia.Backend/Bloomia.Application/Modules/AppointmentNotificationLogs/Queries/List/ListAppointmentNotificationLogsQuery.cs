using Bloomia.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.AppointmentNotificationLogs.Queries.List
{
    public class ListAppointmentNotificationLogsQuery : BasePagedQuery<ListAppointmentNotificationLogsQueryDto>
    {
        public string? Search { get; set; }
        public AppointmentNotificationStatus? Status { get; set; }
        public AppointmentNotificationsType? NotificationType { get; set; }
    }
}
