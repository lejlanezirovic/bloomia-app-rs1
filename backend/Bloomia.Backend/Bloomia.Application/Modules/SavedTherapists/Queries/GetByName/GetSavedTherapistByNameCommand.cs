using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Queries.GetByName
{
    public class GetSavedTherapistByNameCommand:IRequest<List<GetSavedTherapistByNameCommandDto>>
    {
        public string SerachName { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
