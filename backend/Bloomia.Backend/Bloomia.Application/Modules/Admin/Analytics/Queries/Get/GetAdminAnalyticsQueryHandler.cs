using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Admin.Analytics.Queries.Get
{
    public class GetAdminAnalyticsQueryHandler(IAppDbContext ctx) : IRequestHandler<GetAdminAnalyticsQuery, AdminAnalyticsDto>
    {
        public async Task<AdminAnalyticsDto> Handle(GetAdminAnalyticsQuery request, CancellationToken ct)
        {
            if (!request.DateFrom.HasValue || !request.DateTo.HasValue)
            {
                throw new BloomiaBusinessRuleException(
                    "ANALYTICS_DATE_REQUIRED",
                    "Start date and end date are required.");
            }

            var dateFrom = request.DateFrom.Value;
            var dateTo = request.DateTo.Value;

            if (dateFrom > dateTo)
            {
                throw new BloomiaBusinessRuleException("ANALYTICS_DATE_RANGE", "Start date must be before end date.");
            }

            var dateToExclusive = dateTo.AddDays(1);

            var now = DateTime.UtcNow;

            var appointmentsQuery = ctx.Appointments
                .AsNoTracking()
                .Where(x =>
                    x.ScheduledAtUtc >= dateFrom &&
                    x.ScheduledAtUtc < dateToExclusive);

            var appointmentsCount = await appointmentsQuery.CountAsync(ct);

            var completedSessionsCount =
                await appointmentsQuery.CountAsync(
                    x => x.ScheduledAtUtc.AddHours(1) <= now,
                    ct);

            var reviewsQuery = ctx.Reviews
                .AsNoTracking()
                .Where(x =>
                    x.CreatedAtUtc >= dateFrom &&
                    x.CreatedAtUtc < dateToExclusive);

            var averageRating = await reviewsQuery.AverageAsync(x => (float?)x.Rating, ct) ?? 0;

            var clientsCount = await ctx.Clients.AsNoTracking().CountAsync(ct);

            var therapistsCount = await ctx.Therapists.AsNoTracking().CountAsync(ct);

            var activeClientsCount = await appointmentsQuery
                    .Select(x => x.ClientId)
                    .Distinct()
                    .CountAsync(ct);

            var requestLogsQuery = ctx.RequestLogs
                .AsNoTracking()
                .Where(x =>
                    x.CreatedAtUtc >= dateFrom &&
                    x.CreatedAtUtc < dateToExclusive);

            var totalRequests = await requestLogsQuery.CountAsync(ct);

            var averageResponseTimeMs = await requestLogsQuery
                    .AverageAsync(x => (double?)x.DurationMs, ct)
                ?? 0;

            var serverErrorsCount = await requestLogsQuery
                    .CountAsync(x => x.StatusCode >= 500, ct);

            var errorRate = totalRequests == 0 ? 0 : (double)serverErrorsCount / totalRequests * 100;

            return new AdminAnalyticsDto
            {
                Business = new AdminBusinessAnalyticsDto
                {
                    AppointmentsCount = appointmentsCount, 
                    CompletedSessionsCount = completedSessionsCount, 
                    AverageRating = (float)Math.Round(averageRating, 1)
                }, 
                Users = new AdminUserAnalyticsDto
                {
                    ClientsCount = clientsCount, 
                    TherapistsCount = therapistsCount, 
                    ActiveClientsCount = activeClientsCount
                }, 
                SystemPerformance =  new AdminSystemPerformanceDto
                {
                    TotalRequests = totalRequests, 
                    AverageResponseTimeMs = Math.Round(averageResponseTimeMs, 1), 
                    ServerErrorsCount = serverErrorsCount, 
                    ErrorRate = Math.Round(errorRate, 2)
                }
            };
        }
    }
}
