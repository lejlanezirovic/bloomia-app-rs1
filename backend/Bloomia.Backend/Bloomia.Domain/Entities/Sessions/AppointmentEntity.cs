using Bloomia.Domain.Common;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.ReviewsFolder;
using Bloomia.Domain.Entities.TherapistRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Sessions
{
    public class AppointmentEntity:BaseEntity
    {
        public int ClientId { get; set; }
        public ClientEntity Client { get; set; }
        public int TherapistAvailabilityId { get; set; }
        public TherapistAvailabilityEntity TherapistAvailability { get; set; }
        public SessionType SessionType { get; set; }

        public int ReviewId { get; set; }
        public ReviewEntity? Review { get; set; }
        public DateTime BookedAt { get; set; }

        //list
        public List<ChatSessionEntity>? ChatSessions { get; set; } = new List<ChatSessionEntity>();
    }
}
