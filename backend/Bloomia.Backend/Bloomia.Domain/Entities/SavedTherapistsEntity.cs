using Bloomia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities
{
    public class SavedTherapistsEntity:BaseEntity
    {
        public int ClientId { get; set; }
        public ClientEntity Client { get; set; }
        public int TherapistId { get; set; }
        public TherapistEntity Therapist { get; set; }
        public DateTime SavedAt { get; set; }=DateTime.UtcNow;
    }
}
