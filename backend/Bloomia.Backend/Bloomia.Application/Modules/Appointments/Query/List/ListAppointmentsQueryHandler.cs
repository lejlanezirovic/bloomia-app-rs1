using Bloomia.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Query.List
{
    public class ListAppointmentsQueryHandler(IAppDbContext context) : IRequestHandler<ListAppointmentsQuery, PageResult<ListAppointmentsQueryDto>>
    {
        public async Task<PageResult<ListAppointmentsQueryDto>> Handle(ListAppointmentsQuery request, CancellationToken cancellationToken)
        {

            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            if (client == null)
            {
                throw new BloomiaNotFoundException("Client not found");
            }
            var now = DateTime.UtcNow;

            // Intentionally includes cancelled appointments so the client can see their history.
            var appointmentsQuery = context.Appointments
                        .Where(x => x.ClientId == client.Id)
                        .OrderByDescending(x => x.ScheduledAtUtc)
                        .Select(x => new ListAppointmentsQueryDto {
                            AppointmentId=x.Id,
                            ClientName=x.Client.User.Fullname,
                            TherapistName=x.TherapistAvailability.Therapist.User.Fullname,
                            Date=x.TherapistAvailability.Date,
                            Time=x.TherapistAvailability.StartTime,
                            SessionType=x.SessionType.ToString(),
                            Status=x.Status.ToString(),
                            ScheduledAtUtc=x.ScheduledAtUtc,
                            CanCancel=x.Status == AppointmentStatus.Scheduled && x.ScheduledAtUtc > now
                        }).AsNoTracking();

            var result = await PageResult<ListAppointmentsQueryDto>.FromQueryableAsync(appointmentsQuery, request.Paging, cancellationToken);

            foreach (var appointment in result.Items)
            {
                appointment.ScheduledAtUtc = DateTime.SpecifyKind(appointment.ScheduledAtUtc, DateTimeKind.Utc);
            }

            return result;
        }
    }
}
