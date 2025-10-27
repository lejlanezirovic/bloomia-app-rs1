using Bloomia.Domain.Entities.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.TherapistRelated
{
    public class TherapistAvailabilityEntity
    {
        public int Id { get; set; }
        public int TherapistId { get; set; }
        public TherapistEntity Therapist { get; set; }

        public string DayOfWeek { get; set; }//MON, TUE, WED, THU, FRI, SAT, SUN
        public TimeOnly StartAt { get; set; }
        public TimeOnly EndAt { get; set; }
        public bool IsBooked { get; set; }


        public AppointmentEntity? Appointment { get; set; }
        public int AppointmentId { get; set; }
    }
}
