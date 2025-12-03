using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Queries.List
{
    public class ListSavedTherapistsQuery :BasePagedQuery<ListSavedTherapistInfoDto>
    {
        [JsonIgnore]
        public int UserId { get; set; }

    }
}
