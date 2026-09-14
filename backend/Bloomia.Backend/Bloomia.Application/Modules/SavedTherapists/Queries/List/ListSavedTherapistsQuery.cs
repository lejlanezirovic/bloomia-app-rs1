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

        public string? FullName { get; set; }
        public string? Specialization { get; set; }
        public double? MinRating { get; set; }
        public string? TherapyType { get; set; }
        public bool SortByRatingDesc { get; set; } = false;
    }
}
