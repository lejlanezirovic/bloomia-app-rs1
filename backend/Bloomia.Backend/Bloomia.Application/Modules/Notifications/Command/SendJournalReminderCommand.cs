using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Notifications.Command
{
    public class SendJournalReminderCommand:IRequest<string>
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
