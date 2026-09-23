using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.AppointmentNotificationLogs.Queries.List
{
    public class ListAppointmentNotificationLogsQueryHandler(IAppDbContext context) : IRequestHandler<ListAppointmentNotificationLogsQuery, PageResult<ListAppointmentNotificationLogsQueryDto>>
    {
        public async Task<PageResult<ListAppointmentNotificationLogsQueryDto>> Handle(ListAppointmentNotificationLogsQuery request, CancellationToken ct)
        {
            var query = context.AppointmentNotificationLogs
                .AsNoTracking()
                .Include(x => x.Appointment)
                    .ThenInclude(x => x.Client)
                        .ThenInclude(x => x.User)
                .Include(x => x.Appointment)
                    .ThenInclude(x => x.TherapistAvailability)
                        .ThenInclude(x => x.Therapist)
                            .ThenInclude(x => x.User)
                 .AsQueryable();

            if(!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                query = query.Where(x =>
                    (x.Appointment.Client.User.Fullname != null &&
                     x.Appointment.Client.User.Fullname.ToLower().Contains(search)) ||
                     (x.Appointment.TherapistAvailability.Therapist.User.Fullname != null &&
                     x.Appointment.TherapistAvailability.Therapist.User.Fullname
                         .ToLower()
                         .Contains(search))
                    ||
                    x.RecipientEmail.ToLower().Contains(search));
            }

            if(request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            if(request.NotificationType.HasValue)
            {
                query = query.Where(x => x.NotificationType == request.NotificationType.Value);
            }

            var projectedQuery = query.OrderByDescending(x => x.CreatedAtUtc)
                .Select(x => new ListAppointmentNotificationLogsQueryDto
                {
                    Id = x.Id,
                    AppointmentId = x.AppointmentId,
                    RecipientEmail = x.RecipientEmail,
                    Status = x.Status.ToString(),
                    NotificationType = x.NotificationType.ToString(),
                    CreatedAtUtc = x.CreatedAtUtc,
                    SentAtUtc = x.SentAtUtc,
                    ErrorMessage = x.ErrorMessage,
                    ClientName = x.Appointment.Client.User.Fullname ?? (
                            x.Appointment.Client.User.Firstname +
                            " " +
                            x.Appointment.Client.User.Lastname).Trim(),
                    TherapistName = x.Appointment
                            .TherapistAvailability
                            .Therapist
                            .User
                            .Fullname ?? (
                            x.Appointment
                                .TherapistAvailability
                                .Therapist
                                .User
                                .Firstname +
                            " " +
                            x.Appointment
                                .TherapistAvailability
                                .Therapist
                                .User
                                .Lastname).Trim(),
                    ScheduledAtUtc = x.Appointment.ScheduledAtUtc,
                    SessionType = x.Appointment.SessionType.ToString()
                });

            return await PageResult<ListAppointmentNotificationLogsQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
        }
    }
}
