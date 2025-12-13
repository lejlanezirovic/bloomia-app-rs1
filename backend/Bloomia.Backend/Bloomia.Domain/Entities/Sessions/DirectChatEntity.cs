using Bloomia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Sessions
{
    public class DirectChatEntity:BaseEntity
    {
        public int ClientId { get; set; }
        public ClientEntity Client { get; set; }
        public int TherapistId { get; set; }
        public TherapistEntity Therapist { get; set; }
        public List<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
        /*Svaki (client, therapist) par ima najviše jedan chat.

            Chat postoji bez obzira na appointment.

            Slanje poruke radi odmah → ništa ne treba zakazivati.
        
         DirectChat → "besplatno slanje poruka"*/
    }
}
