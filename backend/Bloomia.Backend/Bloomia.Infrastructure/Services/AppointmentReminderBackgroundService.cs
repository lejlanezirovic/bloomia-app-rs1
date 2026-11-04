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
        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(15);

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            logger.LogInformation("Appointment reminder background service started. Interval: {Interval}", Interval);

            using var timer = new PeriodicTimer(Interval);

            try
            {
                do
                {
                    await RunOnceAsync(ct);
                }
                while (await timer.WaitForNextTickAsync(ct));
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Appointment reminder background service is stopping.");
            }
        }

        private async Task RunOnceAsync(CancellationToken ct)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var reminderService = scope.ServiceProvider.GetRequiredService<IAppointmentReminderService>();

                logger.LogInformation("Starting scheduled appointment reminder execution.");
                await reminderService.ProcessDueRemindersAsync(ct);
                logger.LogInformation("Finished scheduled appointment reminder execution.");
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in appointment reminder background service.");
            }
        }
    }
}
