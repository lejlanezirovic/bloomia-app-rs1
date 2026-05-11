using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Commands.DeleteDocument
{
    public sealed class DeleteTherapistDocumentCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int DocumentId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
