using Bloomia.Application.Abstractions;
using Bloomia.Application.Modules.Therapists.Dashboard.Reports;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Services
{
    public class TherapistReportPdfService : ITherapistReportPdfService
    {
        public byte[] GenerateMonthlyReport(TherapistMonthlyReportData data)
        {
            var monthLabel = new DateTime(data.Year, data.Month, 1)
               .ToString("MMMM yyyy", CultureInfo.InvariantCulture);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {

                    page.Margin(40);

                    page.DefaultTextStyle(style =>
                        style.FontSize(11).FontColor("#4A5759"));

                    page.Header()
                        .Column(column =>
                        {
                            column.Item()
                                .Row(row =>
                                {
                                    row.RelativeItem()
                                        .Text("Bloomia")
                                        .FontSize(28)
                                        .Bold()
                                        .FontColor("#4A5759");

                                    row.ConstantItem(50)
                                        .AlignRight()
                                        .Text("PDF")
                                        .Bold()
                                        .FontColor("#4A5759");
                                });

                            column.Item()
                                .PaddingTop(8)
                                .Text(
                                    $"{data.TherapistName} · {data.Specialization}")
                                .FontSize(11);

                            column.Item()
                                .PaddingTop(12)
                                .LineHorizontal(1)
                                .LineColor("#DEDBD2");
                        });

                    page.Content()
                        .PaddingTop(24)
                        .Column(column =>
                        {
                            column.Spacing(18);

                            column.Item()
                                .Text($"Monthly Report · {monthLabel}")
                                .FontSize(19)
                                .Bold();

                            column.Item()
                                .Text("Appointments & Activity")
                                .FontSize(14)
                                .Bold();


                            column.Item()
                                .Row(row =>
                                {
                                    row.RelativeItem()
                                        .Element(container =>
                                            MetricCard(
                                                container,
                                                "Appointments",
                                                data.AppointmentsCount.ToString()));

                                    row.ConstantItem(10);

                                    row.RelativeItem()
                                        .Element(container =>
                                            MetricCard(
                                                container,
                                                "Completed sessions",
                                                data.CompletedSessionsCount.ToString()));

                                    row.ConstantItem(10);

                                    row.RelativeItem()
                                        .Element(container =>
                                            MetricCard(
                                                container,
                                                "Active clients",
                                                data.ActiveClientsCount.ToString()));
                                });


                            column.Item()
                                .PaddingTop(10)
                                .Text("Ratings")
                                .FontSize(14)
                                .Bold();


                            column.Item()
                                .Row(row =>
                                {
                                    row.RelativeItem()
                                        .Element(container =>
                                            MetricCard(
                                                container,
                                                "Average rating",
                                                data.AverageRating.ToString("0.0")));

                                    row.ConstantItem(10);

                                    row.RelativeItem()
                                        .Element(container =>
                                            MetricCard(
                                                container,
                                                "Reviews received",
                                                data.TotalReviews.ToString()));
                                });


                            column.Item()
                                .PaddingTop(8)
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.ConstantColumn(70);
                                    });

                                    AddRatingRow(
                                        table,
                                        "5 stars",
                                        data.FiveStarReviews);

                                    AddRatingRow(
                                        table,
                                        "4 stars",
                                        data.FourStarReviews);

                                    AddRatingRow(
                                        table,
                                        "3 stars",
                                        data.ThreeStarReviews);

                                    AddRatingRow(
                                        table,
                                        "2 stars",
                                        data.TwoStarReviews);

                                    AddRatingRow(
                                        table,
                                        "1 star",
                                        data.OneStarReviews);
                                });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(
                            $"Generated {data.GeneratedAtUtc:dd.MM.yyyy}")
                        .FontSize(9)
                        .FontColor("#6B705C");
                });
            });

            return document.GeneratePdf();
        }

        private static void MetricCard(
            IContainer container,
            string label,
            string value)
        {
            container
                .Background("#EEF2EC")
                .Border(1)
                .BorderColor("#B0C4B1")
                .CornerRadius(8)
                .Padding(14)
                .Column(column =>
                {
                    column.Item()
                        .Text(label)
                        .FontSize(10)
                        .FontColor("#4A5759");

                    column.Item()
                        .PaddingTop(8)
                        .Text(value)
                        .FontSize(21)
                        .Bold()
                        .FontColor("#4A5759");
                });
        }

        private static void AddRatingRow(
            TableDescriptor table,
            string label,
            int value)
        {
            table.Cell()
                .BorderBottom(1)
                .BorderColor("#DEDBD2")
                .PaddingVertical(8)
                .Text(label);

            table.Cell()
                .BorderBottom(1)
                .BorderColor("#DEDBD2")
                .PaddingVertical(8)
                .AlignRight()
                .Text(value.ToString())
                .Bold();
        }
    }
}
