using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Reports
{
    public class TherapistSessionReportData
    {
        public int TherapistId { get; set; }
        public string TherapistName { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public int ClientId { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int AppointmentsCount { get; set; }

        public int CompletedSessionsCount { get; set; }

        public float AverageRating { get; set; }

        public DateTime GeneratedAtUtc { get; set; }
        public List<TherapistSessionReportItemDto> Sessions { get; set; } = [];
    }

    public class TherapistSessionReportItemDto
    {
        public DateTime ScheduledAtUtc { get; set; }

        public string SessionType { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
