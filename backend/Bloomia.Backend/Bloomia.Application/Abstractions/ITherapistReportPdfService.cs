using Bloomia.Application.Modules.Therapists.Dashboard.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Abstractions
{
    public interface ITherapistReportPdfService
    {
        byte[] GenerateMonthlyReport(TherapistMonthlyReportData data);


    }
}
