using Bloomia.Application.Modules.Therapists.Dashboard.Reports.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Reports.Commands.Generate
{
    public class GenerateTherapistReportCommand : IRequest<TherapistReportListItemDto>
    {
        public int ClientId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
