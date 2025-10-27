using Bloomia.Domain.Entities.Identity;
using Bloomia.Domain.Entities.JournalsFolder;
using Bloomia.Domain.Entities.MoodsFolder;
using Bloomia.Domain.Entities.SelfTestsFolder;
using Bloomia.Domain.Entities.Sessions;
using Bloomia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities
{
    public class ClientEntity: BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        //liste
        public  List<SavedTherapistsEntity> SavedTherapists { get; set; } = new List<SavedTherapistsEntity>();
        public List<MoodEntity> Moods { get; set; } = new List<MoodEntity>();
        public List<JournalEntity> Journals { get; set; } = new List<JournalEntity>();
        public List<SelfTestResultEntity> SelfTestResults { get; set; } = new List<SelfTestResultEntity>();
        //sastanci
        public List<AppointmentEntity> Appointments { get; set; } = new List<AppointmentEntity>();

    }
}
