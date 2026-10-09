using Bloomia.Application.Abstractions;
using Bloomia.Application.Modules.Admin.Analytics.Queries.Get;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Services
{
    public class AdminAnalyticsExcelService : IAdminAnalyticsExcelService
    {
        public byte[] Generate(AdminAnalyticsDto data, DateTime dateFrom, DateTime dateTo)
        {
            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Analytics");

            worksheet.Cell("A1").Value = "Bloomia - Admin Analytics Report";
            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("A1").Style.Font.FontSize = 18;

            worksheet.Cell("A2").Value = $"Period: {dateFrom:dd.MM.yyyy} - {dateTo:dd.MM.yyyy}";

            worksheet.Cell("A4").Value = "Business";
            worksheet.Cell("A4").Style.Font.Bold = true;
            worksheet.Cell("A4").Style.Font.FontSize = 14;

            worksheet.Cell("A5").Value = "Metric";
            worksheet.Cell("B5").Value = "Value";

            worksheet.Cell("A6").Value = "Appointments";
            worksheet.Cell("B6").Value = data.Business.AppointmentsCount;

            worksheet.Cell("A7").Value = "Completed sessions";
            worksheet.Cell("B7").Value = data.Business.CompletedSessionsCount;

            worksheet.Cell("A8").Value = "Average rating";
            worksheet.Cell("B8").Value = data.Business.AverageRating;
            worksheet.Cell("B8").Style.NumberFormat.Format = "0.0";

            worksheet.Cell("A10").Value = "Users";
            worksheet.Cell("A10").Style.Font.Bold = true;
            worksheet.Cell("A10").Style.Font.FontSize = 14;

            worksheet.Cell("A11").Value = "Metric";
            worksheet.Cell("B11").Value = "Value";

            worksheet.Cell("A12").Value = "Clients";
            worksheet.Cell("B12").Value = data.Users.ClientsCount;

            worksheet.Cell("A13").Value = "Therapists";
            worksheet.Cell("B13").Value = data.Users.TherapistsCount;

            worksheet.Cell("A14").Value = "Active clients";
            worksheet.Cell("B14").Value = data.Users.ActiveClientsCount;

            worksheet.Cell("A16").Value = "System Performance";
            worksheet.Cell("A16").Style.Font.Bold = true;
            worksheet.Cell("A16").Style.Font.FontSize = 14;

            worksheet.Cell("A17").Value = "Metric";
            worksheet.Cell("B17").Value = "Value";

            worksheet.Cell("A18").Value = "Total API requests";
            worksheet.Cell("B18").Value = data.SystemPerformance.TotalRequests;

            worksheet.Cell("A19").Value = "Average response time";
            worksheet.Cell("B19").Value = data.SystemPerformance.AverageResponseTimeMs;
            worksheet.Cell("B19").Style.NumberFormat.Format = "0.0";

            worksheet.Cell("C19").Value = "ms";

            worksheet.Cell("A20").Value = "Server errors (5xx)";
            worksheet.Cell("B20").Value =
                data.SystemPerformance.ServerErrorsCount;

            worksheet.Cell("A21").Value = "Error rate";
            worksheet.Cell("B21").Value = data.SystemPerformance.ErrorRate;
            worksheet.Cell("B21").Style.NumberFormat.Format = "0.00";

            worksheet.Cell("C21").Value = "%";

            // Basic formatting
            worksheet.Range("A5:B5").Style.Font.Bold = true;
            worksheet.Range("A11:B11").Style.Font.Bold = true;
            worksheet.Range("A17:C17").Style.Font.Bold = true;

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }
    }
}
