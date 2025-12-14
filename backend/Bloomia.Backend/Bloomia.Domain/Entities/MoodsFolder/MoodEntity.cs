using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Common;

namespace Bloomia.Domain.Entities.MoodsFolder
{
    public class MoodEntity : BaseEntity
    {
     
        public int ClientId { get; set; }
        public ClientEntity Client { get; set; }

        public DateTime RecordedTime { get; set; }//kad je zadnji put snimljeno

        public int happiness { get; set; }
        public int sadness { get; set; }
        public int anger { get; set; }
        public int stress { get; set; }
        public int depression { get; set; }
        public int anxiety { get; set; }
        public int WeekNumber { get; set; }

    }
}
