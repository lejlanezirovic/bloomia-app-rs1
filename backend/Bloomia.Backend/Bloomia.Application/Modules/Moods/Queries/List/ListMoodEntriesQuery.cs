using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Moods.Queries.List
{
    public sealed class ListMoodEntriesQuery : BasePagedQuery<ListMoodEntriesQueryDto>
    {
        public int? ClientId { get; set; } //ako je terapeut prijavljen
        
    }
}
