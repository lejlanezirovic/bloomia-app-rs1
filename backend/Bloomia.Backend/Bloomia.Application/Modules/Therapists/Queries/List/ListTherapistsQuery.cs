using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Modules.Users.Queries.List;

namespace Bloomia.Application.Modules.Therapists.Queries.List
{
    public sealed class ListTherapistsQuery : BasePagedQuery<ListTherapistsQueryDto>
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Specialization { get; set; }
        public int? GenderId { get; set; }
        public bool SortByRatingDesc { get; set; } = true;
        public string? Search { get; set; }
    }
}
