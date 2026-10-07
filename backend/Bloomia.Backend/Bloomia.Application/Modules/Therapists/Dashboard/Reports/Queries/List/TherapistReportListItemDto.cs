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
        public int ClientId { get; set; } 
        public string ClientName { get; set; } = string.Empty; 
        public DateTime DateFrom { get; set; } 
        public DateTime DateTo { get; set; } 
        public string FileName { get; set; } = string.Empty; 
        public DateTime GeneratedAtUtc { get; set; } 
        public int AppointmentsCount { get; set; } 
        public int CompletedSessionsCount { get; set; } 
        public float AverageRating { get; set; }
    }
}
