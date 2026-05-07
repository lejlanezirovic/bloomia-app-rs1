using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Commands.Update
{
    public sealed class UpdateTherapistCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Specialization { get; set; }
        public string? Description { get; set; }
        public List<int> TherapyTypeIds { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? LocationId { get; set; }
        
    }
}
