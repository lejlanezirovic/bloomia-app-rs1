using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.TherapistRelated
{
    public class TherapistsTherapyTypesEntity
    {
        public int Id { get; set; }//?
        public int TherapistId { get; set; }
        public TherapistEntity Therapist { get; set; }
        public int TherapyTypeId { get; set; }
        public TherapyTypeEntity TherapyType { get; set; }
    }
}
