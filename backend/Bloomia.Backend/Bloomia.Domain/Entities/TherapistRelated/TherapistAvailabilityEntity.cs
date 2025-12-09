using Bloomia.Domain.Common;
using Bloomia.Domain.Entities.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.TherapistRelated
{
    public class TherapistAvailabilityEntity : BaseEntity
    {
        public int TherapistId { get; set; }
        public TherapistEntity Therapist { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public bool IsBooked { get; set; }

        public AppointmentEntity? Appointment { get; set; }
        public int? AppointmentId { get; set; }
    }
}
