using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Create.CreateForTherapist
{
    public class SendMessageTherapistCommandDto
    {
        public string Note { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
