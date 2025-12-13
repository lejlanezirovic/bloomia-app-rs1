using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Create.CreateForClient
{
    public sealed class SendMessageCommand: IRequest<SendMessageCommandDto>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int TherapistId { get; set; }
        public string Content { get; set; }
    }
}
