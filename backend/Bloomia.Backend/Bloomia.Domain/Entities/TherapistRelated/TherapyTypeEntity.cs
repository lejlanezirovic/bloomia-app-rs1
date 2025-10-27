using Bloomia.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.TherapistRelated
{
    public class TherapyTypeEntity
    {
       public int Id { get; set; }
       public  string TherapyName { get; set; }
       public string? Description { get; set; }

        public List<TherapistsTherapyTypesEntity> TherapistsList { get; set; } = new List<TherapistsTherapyTypesEntity>();
    }
}
