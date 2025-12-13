using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Update.UpdateForTherapist
{
    public class UpdateMessageTherapistCommandDto
    {
        public int DirectChatId { get; set; }
        public int MessageId { get; set; }
        public string UpdatedContent { get; set; }
        public string OldMessage { get; set; }
        public bool IsRead { get; set; }
    }
}
