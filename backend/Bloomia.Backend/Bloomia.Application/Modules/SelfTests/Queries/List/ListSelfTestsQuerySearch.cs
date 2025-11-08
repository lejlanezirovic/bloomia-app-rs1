using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.List
{
    public class ListSelfTestsQuerySearch:IRequest<ListAllSelfTestQuerySearchDto>
    {
        public string? Search { get; init; }
        
    }
}
