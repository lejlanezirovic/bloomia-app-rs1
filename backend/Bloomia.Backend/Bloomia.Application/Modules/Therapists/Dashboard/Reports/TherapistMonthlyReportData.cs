using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Reports
{
    public class TherapistMonthlyReportData
    {
        public string TherapistName { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public int Month { get; set; }

        public int Year { get; set; }

        public int AppointmentsCount { get; set; }

        public int CompletedSessionsCount { get; set; }

        public int ActiveClientsCount { get; set; }

        public float AverageRating { get; set; }

        public int TotalReviews { get; set; }

        public int FiveStarReviews { get; set; }

        public int FourStarReviews { get; set; }

        public int ThreeStarReviews { get; set; }

        public int TwoStarReviews { get; set; }

        public int OneStarReviews { get; set; }

        public DateTime GeneratedAtUtc { get; set; }
    }
}
