using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.MyClients.Queries
{
    public class ListMyClientsQuery : BasePagedQuery<ListMyClientsQueryDto>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string? Search { get; init; }
    }
}
