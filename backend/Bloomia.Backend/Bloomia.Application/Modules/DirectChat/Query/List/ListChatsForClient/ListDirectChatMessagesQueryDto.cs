using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.List.ListChatsForClient
{
    public class ListDirectChatMessagesQueryDto
    {
        public int DirectChatId { get; set; }
        public int TherapistId { get; set; }
        public string ProfileImage { get; set; }
        public string TherapistFullname { get; set; }
        public bool IsReadLAstMessage { get; set; }
    }
}
