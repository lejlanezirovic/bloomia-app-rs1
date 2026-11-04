using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Abstractions;
using Bloomia.Application.Modules.Appointments;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.Sessions;
using Microsoft.Extensions.Logging;

namespace Bloomia.Infrastructure.Services
{
    public class AppointmentReminderService(IAppDbContext context, IEmailService emailService, ILogger<AppointmentReminderService> logger) : IAppointmentReminderService
    {
        private static readonly TimeSpan ReminderLeadTime = TimeSpan.FromHours(24);

        public async Task ProcessDueRemindersAsync(CancellationToken ct)
        {
            var nowUtc = DateTime.UtcNow;
            var windowEndUtc = nowUtc + ReminderLeadTime;

            logger.LogInformation(
                "Processing due appointment reminders. UTC range: {StartUtc} - {EndUtc}",
                nowUtc,
                windowEndUtc);

            var appointmentsToProcess = await context.Appointments
                .Include(x => x.Client)
                    .ThenInclude(x => x.User)
                .Include(x => x.TherapistAvailability)
                    .ThenInclude(x => x.Therapist)
                        .ThenInclude(x => x.User)
                .WhereNotCancelled()
                .Where(x => !x.IsDeleted &&
                        x.ScheduledAtUtc > nowUtc &&
                        x.ScheduledAtUtc <= windowEndUtc &&
                        !context.AppointmentNotificationLogs.Any(l =>
                            l.AppointmentId == x.Id &&
                            l.NotificationType == AppointmentNotificationsType.AppointmentReminder))
                .ToListAsync(ct);

            if (appointmentsToProcess.Count == 0)
            {
                logger.LogInformation("No due appointment reminders found.");
                return;
            }

            logger.LogInformation("Found {ToProcess} due appointment reminders to process.", appointmentsToProcess.Count);

            foreach(var appointment in appointmentsToProcess)
            {
                ct.ThrowIfCancellationRequested();

                var clientEmail = appointment.Client.User.Email;
                var clientName = appointment.Client.User.Fullname ??
                                 $"{appointment.Client.User.Firstname} {appointment.Client.User.Lastname}".Trim();

                var therapistName = appointment.TherapistAvailability.Therapist.User.Fullname ??
                                    $"{appointment.TherapistAvailability.Therapist.User.Firstname} {appointment.TherapistAvailability.Therapist.User.Lastname}".Trim();
            
                if(string.IsNullOrEmpty(clientEmail))
                {
                    logger.LogWarning("Skipping appointment {AppointmentId} because client email is missing.",
                        appointment.Id);
                    context.AppointmentNotificationLogs.Add(new AppointmentNotificationLogEntity
                    {
                        AppointmentId = appointment.Id,
                        RecipientEmail = string.Empty,
                        NotificationType = AppointmentNotificationsType.AppointmentReminder,
                        Status = AppointmentNotificationStatus.Failed,
                        CreatedAtUtc = DateTime.UtcNow,
                        ErrorMessage = "Client email is missing."
                    });

                    await context.SaveChangesAsync(ct);

                    continue;
                }

                var log = new AppointmentNotificationLogEntity
                {
                    AppointmentId = appointment.Id,
                    RecipientEmail = clientEmail,
                    NotificationType = AppointmentNotificationsType.AppointmentReminder,
                    Status = AppointmentNotificationStatus.Pending,
                    CreatedAtUtc = DateTime.UtcNow,
                };

                context.AppointmentNotificationLogs.Add(log);
                await context.SaveChangesAsync(ct);

                try
                {
                    await emailService.SendAppointmentReminderAsync(
                        toEmail: clientEmail,
                        clientName: clientName,
                        therapistName: therapistName,
                        appointmentDate: appointment.TherapistAvailability.Date,
                        appointmentTime: appointment.TherapistAvailability.StartTime,
                        sessionType: appointment.SessionType.ToString(),
                        ct: ct);

                    log.Status = AppointmentNotificationStatus.Sent;
                    log.SentAtUtc = DateTime.UtcNow;
                    log.ErrorMessage = null;

                    await context.SaveChangesAsync(ct);

                    logger.LogInformation("Reminder sent successfully for appointment {AppointmentId} to {RecipientEmail}",
                        appointment.Id,
                        clientEmail);
                }
                catch(Exception ex)
                {
                    log.Status = AppointmentNotificationStatus.Failed;
                    log.ErrorMessage = ex.Message;

                    await context.SaveChangesAsync(ct);

                    logger.LogError(
                        ex,
                        "Failed to send reminder for appointment {AppointmentId} to {RecipientEmail}",
                        appointment.Id,
                        clientEmail);
                }
            }

        }
    }
}
