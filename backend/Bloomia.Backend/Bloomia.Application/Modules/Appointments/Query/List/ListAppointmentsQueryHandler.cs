using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Query.List
{
    public class ListAppointmentsQueryHandler(IAppDbContext context) : IRequestHandler<ListAppointmentsQuery, List< ListAppointmentsQueryDto>>
    {
        public async Task<List<ListAppointmentsQueryDto>> Handle(ListAppointmentsQuery request, CancellationToken cancellationToken)
        {

            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            if (client == null)
            {
                throw new BloomiaNotFoundException("Client not found");
            }
            var appointments = context.Appointments.Include(x => x.Client).ThenInclude(x => x.User)
                        .Include(x => x.TherapistAvailability).ThenInclude(x => x.Therapist).ThenInclude(x => x.User)
                        .Where(x => x.ClientId == client.Id)
                        .Select(x => new ListAppointmentsQueryDto { 
                            AppointmentId=x.Id,
                            ClientName=x.Client.User.Fullname,
                            TherapistName=x.TherapistAvailability.Therapist.User.Fullname,
                            Date=x.TherapistAvailability.Date,
                            Time=x.TherapistAvailability.StartTime,
                            SessionType=x.SessionType.ToString()         
                        }).AsNoTracking();

            //var dto = new ListAppointmentsQueryDto();
            //dto.ClientAppointments.AddRange(appointments);
            //return dto;
            return appointments.ToList();
        }
    }
}
