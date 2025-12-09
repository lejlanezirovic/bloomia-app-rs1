using Bloomia.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Command.Create
{
    public class CreateAppoinmentCommand:IRequest<CreateAppointmentCommandDto>
    {
        public int TherapistAvailabilityId { get; set; }
        public SessionType SessionType { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
