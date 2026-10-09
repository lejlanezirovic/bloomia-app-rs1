using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Articles.Queries.List
{
    public class ListArticlesQuery : BasePagedQuery<ListArticlesQueryDto>
    {
        public string? Title { get; init; }
        public string? Content { get; init; }
        public string? AdminName { get; init; }
        public DateTime? DateFrom { get; init; }
        public DateTime? DateTo { get; init; }
    }
}
