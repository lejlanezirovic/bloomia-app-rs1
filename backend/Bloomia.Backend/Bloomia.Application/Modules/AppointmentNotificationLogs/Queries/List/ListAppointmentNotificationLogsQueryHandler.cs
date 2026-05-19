using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.AppointmentNotificationLogs.Queries.List
{
    public class ListAppointmentNotificationLogsQueryHandler(IAppDbContext context) : IRequestHandler<ListAppointmentNotificationLogsQuery, List<ListAppointmentNotificationLogsQueryDto>>
    {
        public async Task<List<ListAppointmentNotificationLogsQueryDto>> Handle(ListAppointmentNotificationLogsQuery request, CancellationToken ct)
        {
            var logs = await context.AppointmentNotificationLogs
                        .AsNoTracking()
                        .Include(x => x.Appointment)
                            .ThenInclude(x => x.Client)
                                .ThenInclude(x => x.User)
                        .Include(x => x.Appointment)
                            .ThenInclude(x => x.TherapistAvailability)
                                .ThenInclude(x => x.Therapist)
                                    .ThenInclude(x => x.User)
                        .OrderByDescending(x => x.CreatedAtUtc)
                        .Select(x => new ListAppointmentNotificationLogsQueryDto
                        {
                            Id = x.Id,
                            AppointmentId = x.AppointmentId,
                            RecipientEmail = x.RecipientEmail,
                            NotificationType = x.NotificationType.ToString(),
                            Status = x.Status.ToString(),
                            CreatedAtUtc = x.CreatedAtUtc,
                            SentAtUtc = x.SentAtUtc,
                            ErrorMessage = x.ErrorMessage,
                            ClientName = x.Appointment.Client.User.Fullname ??
                                        $"{x.Appointment.Client.User.Firstname} {x.Appointment.Client.User.Lastname}".Trim(),
                            TherapistName = x.Appointment.TherapistAvailability.Therapist.User.Fullname ??
                                        $"{x.Appointment.TherapistAvailability.Therapist.User.Firstname} {x.Appointment.TherapistAvailability.Therapist.User.Lastname}".Trim(),
                            ScheduledAtUtc = x.Appointment.ScheduledAtUtc,
                            SessionType = x.Appointment.SessionType.ToString()
                        }).ToListAsync(ct);

            return logs;
        }
    }
}
