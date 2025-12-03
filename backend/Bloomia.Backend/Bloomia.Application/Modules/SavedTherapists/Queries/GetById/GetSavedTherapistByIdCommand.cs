using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Queries.GetById
{
    public class GetSavedTherapistByIdCommand: IRequest<GetSavedTherapistByIdCommandDto>
    {
        [JsonIgnore]
        public int UserId { get; set; } 
        public int TherapistId { get; set; }
    }
}
