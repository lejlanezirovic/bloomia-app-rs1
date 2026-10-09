using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Admin.Analytics.Queries.Export
{
    public class AdminAnalyticsFileDto
    {
        public byte[] Content { get; set; } = [];
        public string FileName { get; set; } = string.Empty;
    }
}
