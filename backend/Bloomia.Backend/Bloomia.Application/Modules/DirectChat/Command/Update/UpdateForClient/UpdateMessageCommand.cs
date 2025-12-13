using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Update.UpdateForClient
{
    public sealed class UpdateMessageCommand:IRequest<UpdateMessageCommandDto>
    {
        public int DirectChatId { get; set; }
        public int MessageId { get; set; }
        public string NewContent { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
