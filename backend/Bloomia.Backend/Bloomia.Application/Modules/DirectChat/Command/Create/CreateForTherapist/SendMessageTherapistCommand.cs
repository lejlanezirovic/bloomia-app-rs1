using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Create.CreateForTherapist
{
    public sealed class SendMessageTherapistCommand:IRequest<SendMessageTherapistCommandDto>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public string Content { get; set; }
    }
}
