using Bloomia.Domain.Common;
using Bloomia.Domain.Entities.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.ReviewsFolder
{
    public class ReviewEntity : BaseEntity
    {
        //public int ClientId { get; set; }
        //public ClientEntity Client { get; set; }
        //public int TherapistId { get; set; }
        //public TherapistEntity Therapist { get; set; }

        public int AppointmentId { get; set; }
        public AppointmentEntity Appointment { get; set; }
        public int Rating { get; set; } // 1 to 5
        public string? Comment { get; set; }
    }
}
