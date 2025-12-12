using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Moods.Queries.List
{
    public sealed class ListMoodEntriesQueryDto
    {
        public int Id { get; set; }
        public DateTime RecordedTime { get; set; }
        public int Happiness { get; set; }
        public int Sadness { get; set; }
        public int Anger { get; set; }
        public int Stress { get; set; }
        public int Depression { get; set; }
        public int Anxiety { get; set; }
        public int WeekNumber { get; set;  }
    }
}
