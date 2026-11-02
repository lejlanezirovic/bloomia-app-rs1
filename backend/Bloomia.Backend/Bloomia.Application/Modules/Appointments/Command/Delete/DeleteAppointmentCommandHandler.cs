using Bloomia.Domain.Entities;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.Sessions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Command.Delete
{
    public class DeleteAppointmentCommandHandler(IAppDbContext context, IEmailService emailService, ILogger<DeleteAppointmentCommandHandler> logger) : IRequestHandler<DeleteAppointmentCommand, string>
    {
        public async Task<string> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            var therapist = await context.Therapists.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);

            var appointment = await context.Appointments.Include(x=>x.TherapistAvailability)
                .ThenInclude(x=>x.Therapist).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == request.AppointmentId, cancellationToken);

            if(appointment==null || appointment.IsDeleted == true)
            {
                throw new BloomiaNotFoundException("Appointment not found");
            }

            var isClientOwner = client != null && appointment.ClientId == client.Id;
            var isTherapistOwner=therapist!=null && appointment.TherapistAvailability!=null
                && appointment.TherapistAvailability.TherapistId==therapist.Id;

            if (!isClientOwner && !isTherapistOwner)
            {
                throw new BloomiaNotFoundException("Appointment not found(Unauthorized)!");
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new BloomiaConflictException("Appointment is already cancelled.");
            }

            if (appointment.ScheduledAtUtc <= DateTime.UtcNow)
            {
                throw new BloomiaBusinessRuleException("APPOINTMENT_STARTED", "An appointment that has already started cannot be cancelled.");
            }

            AppointmentNotificationLogEntity? notificationLog = null;

            await using var transaction=await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                // Cancellation keeps the row (no soft delete) so it stays in the client's history.
                appointment.Status = AppointmentStatus.Cancelled;
                if(appointment.TherapistAvailability!=null)
                {
                    appointment.TherapistAvailability.IsBooked = false;
                    appointment.TherapistAvailability.ModifiedAtUtc = DateTime.UtcNow;
                }

                if (isClientOwner && appointment.TherapistAvailability != null)
                {
                    var clientEmail = client!.User.Email;
                    var hasClientEmail = !string.IsNullOrWhiteSpace(clientEmail);

                    notificationLog = new AppointmentNotificationLogEntity
                    {
                        AppointmentId = appointment.Id,
                        RecipientEmail = clientEmail ?? string.Empty,
                        NotificationType = AppointmentNotificationsType.AppointmentCancellation,
                        Status = hasClientEmail ? AppointmentNotificationStatus.Pending : AppointmentNotificationStatus.Failed,
                        ErrorMessage = hasClientEmail ? null : "Client email is missing.",
                        CreatedAtUtc = DateTime.UtcNow
                    };
                    context.AppointmentNotificationLogs.Add(notificationLog);
                }

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            if (notificationLog?.Status == AppointmentNotificationStatus.Pending)
            {
                await SendCancellationEmailAsync(client!, appointment, notificationLog, cancellationToken);
            }
            else if (notificationLog != null)
            {
                logger.LogWarning(
                    "Cancellation email skipped for appointment {AppointmentId} because client email is missing.",
                    appointment.Id);
            }

            return "Appointment cancelled successfully";
        }

        private async Task SendCancellationEmailAsync(ClientEntity client, AppointmentEntity appointment,
            AppointmentNotificationLogEntity notificationLog, CancellationToken cancellationToken)
        {
            try
            {
                var availability = appointment.TherapistAvailability;
                var clientName = client.User.Fullname ?? $"{client.User.Firstname} {client.User.Lastname}".Trim();
                var therapistName = availability.Therapist.User.Fullname ??
                                    $"{availability.Therapist.User.Firstname} {availability.Therapist.User.Lastname}".Trim();

                await emailService.SendAppointmentCancellationAsync(
                    toEmail: notificationLog.RecipientEmail,
                    clientName: clientName,
                    therapistName: therapistName,
                    appointmentDate: availability.Date,
                    appointmentTime: availability.StartTime,
                    sessionType: appointment.SessionType.ToString(),
                    ct: cancellationToken);

                notificationLog.Status = AppointmentNotificationStatus.Sent;
                notificationLog.SentAtUtc = DateTime.UtcNow;
                notificationLog.ErrorMessage = null;
            }
            catch (Exception ex)
            {
                notificationLog.Status = AppointmentNotificationStatus.Failed;
                notificationLog.ErrorMessage = ex.Message;

                logger.LogError(ex,
                                "Failed to send cancellation email for appointment {AppointmentId} and client {ClientId}",
                                appointment.Id, client.Id);
            }

            try
            {
                await context.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                                "Failed to save cancellation email status for appointment {AppointmentId}",
                                appointment.Id);
            }
        }
    }
}
