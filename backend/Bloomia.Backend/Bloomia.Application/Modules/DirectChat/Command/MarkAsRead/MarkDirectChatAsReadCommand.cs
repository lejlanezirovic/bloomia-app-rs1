using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.MarkAsRead
{
    public class MarkDirectChatAsReadCommand : IRequest
    {
        public int DirectChatId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
