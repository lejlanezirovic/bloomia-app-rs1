using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Reports.Queries.List
{
    public class TherapistReportListItemDto
    {
        public int Id { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string FileName { get; set; } = string.Empty;

        public DateTime GeneratedAtUtc { get; set; }

        public int AppointmentsCount { get; set; }

        public int CompletedSessionsCount { get; set; }

        public int ActiveClientsCount { get; set; }

        public float AverageRating { get; set; }

        public int TotalReviews { get; set; }
    }
}
