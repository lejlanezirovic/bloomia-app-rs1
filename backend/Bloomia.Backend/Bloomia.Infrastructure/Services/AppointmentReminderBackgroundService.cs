using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bloomia.Infrastructure.Services
{
    public class AppointmentReminderBackgroundService(IServiceScopeFactory scopeFactory, ILogger<AppointmentReminderBackgroundService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            var bihTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

            logger.LogInformation("Appointment reminder background service started.");

            while(!ct.IsCancellationRequested)
            {
                try
                {
                    var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, bihTimeZone);

                    var nextRunLocal = new DateTime(
                        nowLocal.Year,
                        nowLocal.Month,
                        nowLocal.Day,
                        9, 0, 0);

                    if (nowLocal >= nextRunLocal)
                        nextRunLocal = nextRunLocal.AddDays(1);

                    var delay = nextRunLocal - nowLocal;

                    logger.LogInformation(
                        "Next appointment reminder run scheduled for {NextRunLocal}. Delay: {Delay}",
                        nextRunLocal,
                        delay);

                    await Task.Delay(delay, ct);

                    using var scope = scopeFactory.CreateScope();

                    var reminderService = scope.ServiceProvider.GetRequiredService<IAppointmentReminderService>();

                    logger.LogInformation("Starting scheduled appointment reminder execution.");
                    await reminderService.ProcessTomorrowRemindersAsync(ct);
                    logger.LogInformation("Finished scheduled appointment reminder execution.");

                }
                catch(OperationCanceledException)
                {
                    logger.LogInformation("Appointment reminder background service is stopping.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error in appointment reminder background service.");

                    await Task.Delay(TimeSpan.FromMinutes(5), ct);
                }
            }
        }
    }
}
