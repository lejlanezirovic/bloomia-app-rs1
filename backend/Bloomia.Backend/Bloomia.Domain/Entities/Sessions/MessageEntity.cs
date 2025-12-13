using Bloomia.Domain.Common;
using Bloomia.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Sessions
{
    public class MessageEntity:BaseEntity
    {
        public int? DirectChatId { get; set; }
        public DirectChatEntity? DirectChat { get; set; }

        public int? ChatSessionId { get; set; }
        public ChatSessionEntity? ChatSession { get; set; }

        public int SenderId { get; set; } // Could be ClientId or TherapistId
        public SenderType SenderType { get; set; } // "Client" or "Therapist"
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool isRead { get; set; } = false;
    }
}
