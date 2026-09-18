using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Reports.Queries.GetFile
{
    public class TherapistReportFileDto
    {
        public byte[] Content { get; set; } = [];
        public string FileName { get; set; } = string.Empty;
    }
}
