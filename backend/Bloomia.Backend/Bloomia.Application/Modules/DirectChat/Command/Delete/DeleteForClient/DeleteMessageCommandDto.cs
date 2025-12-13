using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Delete.DeleteForClient
{
    public class DeleteMessageCommandDto
    {
        public bool IsDeleted { get; set; }
        public string Message { get; set; }
        public string SenderType { get; set; }
    }
}
