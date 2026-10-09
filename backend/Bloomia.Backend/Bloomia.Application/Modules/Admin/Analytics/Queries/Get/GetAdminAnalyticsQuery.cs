using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Admin.Analytics.Queries.Get
{
    public class GetAdminAnalyticsQuery : IRequest<AdminAnalyticsDto>
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
