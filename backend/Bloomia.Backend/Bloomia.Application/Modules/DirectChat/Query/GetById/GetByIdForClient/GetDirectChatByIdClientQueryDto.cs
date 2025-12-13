using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.NewFolder.GetById
{
    public class GetDirectChatByIdClientQueryDto
    {
        public int TherapistId { get; set; }
        public string TherapistFullname { get; set; }
        public string ProfileImage { get; set; }
        public List<MessageDto> Messages { get; set; }=new List<MessageDto>();
    }
    public class MessageDto
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public string SenderType  { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
