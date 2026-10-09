using Bloomia.Application.Modules.Admin.Analytics.Queries.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Admin.Analytics.Queries.Export
{
    public class ExportAdminAnalyticsQueryHandler(ISender sender, IAdminAnalyticsExcelService excelService) : IRequestHandler<ExportAdminAnalyticsQuery, AdminAnalyticsFileDto>
    {
        public async Task<AdminAnalyticsFileDto> Handle(ExportAdminAnalyticsQuery request, CancellationToken ct)
        {
            if (!request.DateFrom.HasValue || !request.DateTo.HasValue)
            {
                throw new BloomiaBusinessRuleException(
                    "ANALYTICS_DATE_REQUIRED",
                    "Start date and end date are required.");
            }

            var dateFrom = request.DateFrom.Value;
            var dateTo = request.DateTo.Value;

            var analytics = await sender.Send(new GetAdminAnalyticsQuery
            {
                DateFrom = dateFrom,
                DateTo = dateTo,
            }, ct);

            var content = excelService.Generate(analytics, dateFrom, dateTo);

            return new AdminAnalyticsFileDto
            {
                Content = content,
                FileName =$"Bloomia-Analytics-" +
                        $"{request.DateFrom:yyyy-MM-dd}-" +
                        $"{request.DateTo:yyyy-MM-dd}.xlsx"
            };
        }
    }
}
