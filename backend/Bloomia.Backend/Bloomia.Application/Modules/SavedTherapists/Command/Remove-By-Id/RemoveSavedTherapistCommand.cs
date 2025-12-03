using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Command.Remove
{
    public class RemoveSavedTherapistCommand: IRequest<string>
    {
        [JsonIgnore]
        public int UserId { get; set; } 

        public int TherapistId { get; set; }
    }
}
