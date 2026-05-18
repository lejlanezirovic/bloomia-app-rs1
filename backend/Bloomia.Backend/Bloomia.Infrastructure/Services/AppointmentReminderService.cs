using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Abstractions;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.Sessions;
using FluentValidation.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bloomia.Infrastructure.Services
{
    public class AppointmentReminderService(IAppDbContext context, IEmailService emailService, ILogger<AppointmentReminderService> logger) : IAppointmentReminderService
    {
        public async Task ProcessTomorrowRemindersAsync(CancellationToken ct)
        {
            var bihTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, bihTimeZone);

            var tomorrowLocalDate = DateOnly.FromDateTime(nowLocal.Date.AddDays(1));

            var tomorrowStartLocal = tomorrowLocalDate.ToDateTime(TimeOnly.MinValue);
            var dayAfterStartLocal = tomorrowLocalDate.AddDays(1).ToDateTime(TimeOnly.MinValue);

            var tomorrowStartUtc = TimeZoneInfo.ConvertTimeToUtc(tomorrowStartLocal, bihTimeZone);
            var dayAfterStartUtc = TimeZoneInfo.ConvertTimeToUtc(dayAfterStartLocal, bihTimeZone);

            logger.LogInformation(
                "Processing appointment reminders for local date {TomorrowDate}. UTC range: {StartUtc} - {EndUtc}",
                tomorrowLocalDate,
                tomorrowStartUtc,
                dayAfterStartUtc);

            var appointments = await context.Appointments
                .Include(x => x.Client)
                    .ThenInclude(x => x.User)
                .Include(x => x.TherapistAvailability)
                    .ThenInclude(x => x.Therapist)
                        .ThenInclude(x => x.User)
                .Where(x => !x.IsDeleted &&
                        x.ScheduledAtUtc >= tomorrowStartUtc &&
                        x.ScheduledAtUtc < dayAfterStartUtc)
                .ToListAsync(ct);

            if (appointments.Count == 0)
            {
                logger.LogInformation("No appointments found for tomorrow reminders.");
                return;
            }

            var appointmentIds = appointments.Select(x => x.Id).ToList();

            var alreadySentAppointmentIds = await context.AppointmentNotificationLogs
                .Where(x =>
                        appointmentIds.Contains(x.AppointmentId) &&
                        x.NotificationType == AppointmentNotificationsType.AppointmentReminder &&
                        x.Status == AppointmentNotificationStatus.Sent)
                .Select(x => x.AppointmentId)
                .Distinct()
                .ToListAsync(ct);

            var appointmentsToProcess = appointments
                .Where(x => !alreadySentAppointmentIds.Contains(x.Id)).ToList();

            logger.LogInformation(
                "Found {Total} appointments, {AlreadySent} already reminded, {ToProcess} to process.",
                appointments.Count,
                alreadySentAppointmentIds.Count,
                appointmentsToProcess.Count);

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
