using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Client.Queries.List
{
    public sealed class ListClientsQuery : BasePagedQuery<ListClientsQueryDto>
    {
        public string? Search {  get; init; }
    }
}
