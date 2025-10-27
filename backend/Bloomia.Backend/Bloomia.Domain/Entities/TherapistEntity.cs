using Bloomia.Domain.Entities.Basics;
using Bloomia.Domain.Entities.Identity;
using Bloomia.Domain.Entities.Sessions;
using Bloomia.Domain.Entities.TherapistRelated;
using Bloomia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities
{
    public class TherapistEntity: BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public string Specialization { get; set; }
        public string Description { get; set; }
        public float RatingAvg { get; set; }
        public bool isVerified { get; set; }
        public int DocumentId { get; set; }
        public DocumentEntity Document { get; set; }

        //liste
        //1. 
        public List<TherapistsTherapyTypesEntity> MyTherapyTypesList { get; set; } = new List<TherapistsTherapyTypesEntity>();
        //2. 
        public List<TherapistAvailabilityEntity> Availability { get; set; } = new List<TherapistAvailabilityEntity>();
         public List<SavedTherapistsEntity> SavedByClients { get; set; } = new List<SavedTherapistsEntity>();

    }
}
