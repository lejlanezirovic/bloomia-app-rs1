using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Sessions
{
    public class ChatSessionEntity
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public AppointmentEntity Appointment { get; set; }

        //client i therapist 
        public List<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    }
}
