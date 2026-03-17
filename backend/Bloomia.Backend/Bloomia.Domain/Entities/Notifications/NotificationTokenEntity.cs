using Bloomia.Domain.Common;
using Bloomia.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Notifications
{
    public class NotificationTokenEntity:BaseEntity
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public string Token { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;


    }
}
