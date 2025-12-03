using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Command.Add
{
    public class AddTherapistToSavedTherapistsCommand: IRequest<AddTherapistToSavedTherapistsCommandDto>
    {
        //get all therapists
        public int TherapistId { get; init; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
