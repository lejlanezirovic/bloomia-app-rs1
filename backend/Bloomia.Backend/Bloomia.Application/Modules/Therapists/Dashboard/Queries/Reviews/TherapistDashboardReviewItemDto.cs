using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Queries.Reviews
{
    public class TherapistDashboardReviewItemDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
    }
}
