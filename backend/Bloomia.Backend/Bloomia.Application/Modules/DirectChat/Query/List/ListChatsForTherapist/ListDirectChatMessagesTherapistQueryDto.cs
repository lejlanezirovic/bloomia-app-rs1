using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.List.ListChatsForTherapist
{
    public class ListDirectChatMessagesTherapistQueryDto
    {
        public int DirectChatId { get; set; }
        public int ClientId { get; set; }
        public string ProfileImage { get; set; }
        public string ClientFullname { get; set; }
        public bool IsLastMessageRead { get; set; }
    }
}
