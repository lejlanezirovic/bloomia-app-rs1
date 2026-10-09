using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Admin.Analytics.Queries.Get
{
    public class AdminAnalyticsDto
    {
        public AdminBusinessAnalyticsDto Business { get; set; } = new();
        public AdminUserAnalyticsDto Users { get; set; } = new();
        public AdminSystemPerformanceDto SystemPerformance { get; set; } = new();
    }

    public class AdminBusinessAnalyticsDto
    {
        public int AppointmentsCount { get; set; }

        public int CompletedSessionsCount { get; set; }

        public float AverageRating { get; set; }
    }
    public class AdminUserAnalyticsDto
    {
        public int ClientsCount { get; set; }

        public int TherapistsCount { get; set; }

        public int ActiveClientsCount { get; set; }
    }
    public class AdminSystemPerformanceDto
    {
        public int TotalRequests { get; set; }

        public double AverageResponseTimeMs { get; set; }

        public int ServerErrorsCount { get; set; }

        public double ErrorRate { get; set; }
    }
}
