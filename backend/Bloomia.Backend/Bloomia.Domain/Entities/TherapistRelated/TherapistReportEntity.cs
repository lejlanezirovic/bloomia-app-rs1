using Bloomia.Domain.Common;
using Microsoft.AspNetCore.SignalR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.TherapistRelated
{
    public class TherapistReportEntity : BaseEntity
    {
        public int TherapistId { get; set; }
        public TherapistEntity? Therapist { get; set; }
        public int ClientId { get; set; }
        public ClientEntity? Client { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTime GeneratedAtUtc { get; set; }
        public int AppointmentsCount { get; set; }

        public int CompletedSessionsCount { get; set; }

        public float AverageRating { get; set; }
    }
}
