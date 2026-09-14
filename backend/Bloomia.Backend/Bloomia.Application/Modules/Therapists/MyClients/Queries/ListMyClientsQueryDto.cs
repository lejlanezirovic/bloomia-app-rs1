using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.MyClients.Queries
{
    public class ListMyClientsQueryDto
    {
        public int ClientId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime? NextAppointmentAtUtc { get; set; }
        public int? DirectChatId { get; set; }
    }
}
