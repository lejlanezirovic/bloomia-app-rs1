using Bloomia.Application.Abstractions;
using Bloomia.Application.Modules.Therapists.Dashboard.Reports;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Bloomia.Infrastructure.Services
{
    public class TherapistReportPdfService : ITherapistReportPdfService
    {
        public byte[] GenerateSessionReport(TherapistSessionReportData data)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.DefaultTextStyle(style =>
                        style
                            .FontSize(11)
                            .FontColor("#4A5759"));

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

                                    row.ConstantItem(100)
                                        .AlignRight()
                                        .Column(info =>
                                        {
                                            info.Item()
                                                .Text(
                                                    $"Generated: {data.GeneratedAtUtc:dd.MM.yyyy}")
                                                .FontSize(8);

                                            info.Item()
                                                .Text("Session Report")
                                                .FontSize(8);
                                        });
                                });

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
                                .Text("Therapy Session Report")
                                .FontSize(19)
                                .Bold();

                            column.Item()
                                .Background("#EEF2EC")
                                .Border(1)
                                .BorderColor("#DEDBD2")
                                .CornerRadius(8)
                                .Padding(14)
                                .Column(info =>
                                {
                                    info.Spacing(5);

                                    info.Item()
                                        .Text(text =>
                                        {
                                            text.Span("Therapist: ")
                                                .Bold();

                                            text.Span(
                                                $"{data.TherapistName}");
                                        });

                                    info.Item()
                                        .Text(text =>
                                        {
                                            text.Span("Specialization: ")
                                                .Bold();

                                            text.Span(
                                                data.Specialization);
                                        });

                                    info.Item()
                                        .Text(text =>
                                        {
                                            text.Span("Client: ")
                                                .Bold();

                                            text.Span(
                                                $"{data.ClientName}");
                                        });

                                    info.Item()
                                        .Text(text =>
                                        {
                                            text.Span("Period: ")
                                                .Bold();

                                            text.Span(
                                                $"{data.DateFrom:dd.MM.yyyy} - {data.DateTo:dd.MM.yyyy}");
                                        });
                                });

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
                                                data.AppointmentsCount
                                                    .ToString()));

                                    row.ConstantItem(10);

                                    row.RelativeItem()
                                        .Element(container =>
                                            MetricCard(
                                                container,
                                                "Completed sessions",
                                                data.CompletedSessionsCount
                                                    .ToString()));

                                    row.ConstantItem(10);

                                    row.RelativeItem()
                                        .Element(container =>
                                            MetricCard(
                                                container,
                                                "Average rating",
                                                data.AverageRating
                                                    .ToString("0.0")));
                                });

                            column.Item()
                                .PaddingTop(10)
                                .Text("Session overview")
                                .FontSize(14)
                                .Bold();

                            if (data.Sessions.Count == 0)
                            {
                                column.Item()
                                    .Text(
                                        "No sessions were found for the selected client and period.")
                                    .Italic()
                                    .FontColor("#6B705C");
                            }
                            else
                            {
                                column.Item()
                                    .Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.ConstantColumn(95);
                                            columns.ConstantColumn(60);
                                            columns.RelativeColumn();
                                            columns.ConstantColumn(85);
                                        });

                                        table.Header(header =>
                                        {
                                            header.Cell()
                                                .Element(TableHeaderCell)
                                                .Text("Date");

                                            header.Cell()
                                                .Element(TableHeaderCell)
                                                .Text("Time");

                                            header.Cell()
                                                .Element(TableHeaderCell)
                                                .Text("Session type");

                                            header.Cell()
                                                .Element(TableHeaderCell)
                                                .Text("Status");
                                        });

                                        foreach (var session in data.Sessions)
                                        {
                                            var utcDate = DateTime.SpecifyKind(session.ScheduledAtUtc, DateTimeKind.Utc);
                                            var localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timeZone);

                                            table.Cell()
                                                .Element(TableCell)
                                                .Text(localDate.ToString("dd.MM.yyyy"));

                                            table.Cell()
                                                .Element(TableCell)
                                                .Text(localDate.ToString("HH:mm"));

                                            table.Cell()
                                                .Element(TableCell)
                                                .Text(
                                                    session.SessionType);

                                            table.Cell()
                                                .Element(TableCell)
                                                .Text(
                                                    session.Status);
                                        }
                                    });
                            }
                        });

                    page.Footer()
                        .Row(row =>
                        {
                            row.RelativeItem()
                                .Text("Bloomia · confidential")
                                .FontSize(8)
                                .FontColor("#6B705C");

                            row.RelativeItem()
                                .AlignRight()
                                .DefaultTextStyle(style =>
                                    style
                                        .FontSize(8)
                                        .FontColor("#6B705C"))
                                .Text(text =>
                                {
                                    text.Span("Page ");
                                    text.CurrentPageNumber();
                                    text.Span(" of ");
                                    text.TotalPages();
                                });
                        });
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

        private static IContainer TableHeaderCell(
            IContainer container)
        {
            return container
                .Background("#4A5759")
                .PaddingVertical(7)
                .PaddingHorizontal(6)
                .DefaultTextStyle(
                    x => x
                        .FontColor("#FFFFFF")
                        .Bold()
                        .FontSize(9));
        }

        private static IContainer TableCell(
            IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor("#DEDBD2")
                .PaddingVertical(7)
                .PaddingHorizontal(6)
                .DefaultTextStyle(
                    x => x.FontSize(9));
        }
    }
}