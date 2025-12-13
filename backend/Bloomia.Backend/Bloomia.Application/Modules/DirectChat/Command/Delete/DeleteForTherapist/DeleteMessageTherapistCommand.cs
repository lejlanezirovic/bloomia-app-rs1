using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Delete.DeleteForTherapist
{
    public sealed class DeleteMessageTherapistCommand:IRequest<DeleteMessageTherapistCommandDto>
    {
        public int DirectChatId { get; set; }
        public int MessageId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
