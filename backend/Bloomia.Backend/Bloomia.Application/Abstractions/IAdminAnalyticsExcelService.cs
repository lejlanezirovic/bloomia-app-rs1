using Bloomia.Application.Modules.Admin.Analytics.Queries.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Abstractions
{
    public interface IAdminAnalyticsExcelService
    {
        byte[] Generate(AdminAnalyticsDto data, DateTime dateFrom, DateTime dateTo);
    }
}
