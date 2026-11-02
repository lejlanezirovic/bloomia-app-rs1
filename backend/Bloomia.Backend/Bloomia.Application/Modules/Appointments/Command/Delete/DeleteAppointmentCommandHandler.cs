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
                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }catch(Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            if (isClientOwner && appointment.TherapistAvailability != null)
            {
                await SendCancellationEmailAsync(client!, appointment, cancellationToken);
            }

            return "Appointment cancelled successfully";
        }

        private async Task SendCancellationEmailAsync(ClientEntity client, AppointmentEntity appointment, CancellationToken cancellationToken)
        {
            var clientEmail = client.User.Email;

            if (string.IsNullOrWhiteSpace(clientEmail))
            {
                context.AppointmentNotificationLogs.Add(new AppointmentNotificationLogEntity
                {
                    AppointmentId = appointment.Id,
                    RecipientEmail = string.Empty,
                    NotificationType = AppointmentNotificationsType.AppointmentCancellation,
                    Status = AppointmentNotificationStatus.Failed,
                    CreatedAtUtc = DateTime.UtcNow,
                    ErrorMessage = "Client email is missing."
                });

                await context.SaveChangesAsync(cancellationToken);

                logger.LogWarning(
                    "Cancellation email skipped for appointment {AppointmentId} because client email is missing.",
                    appointment.Id);
                return;
            }

            var notificationLog = new AppointmentNotificationLogEntity
            {
                AppointmentId = appointment.Id,
                RecipientEmail = clientEmail,
                NotificationType = AppointmentNotificationsType.AppointmentCancellation,
                Status = AppointmentNotificationStatus.Pending,
                CreatedAtUtc = DateTime.UtcNow
            };

            context.AppointmentNotificationLogs.Add(notificationLog);
            await context.SaveChangesAsync(cancellationToken);

            try
            {
                var availability = appointment.TherapistAvailability;
                var clientName = client.User.Fullname ?? $"{client.User.Firstname} {client.User.Lastname}".Trim();
                var therapistName = availability.Therapist.User.Fullname ??
                                    $"{availability.Therapist.User.Firstname} {availability.Therapist.User.Lastname}".Trim();

                await emailService.SendAppointmentCancellationAsync(
                    toEmail: clientEmail,
                    clientName: clientName,
                    therapistName: therapistName,
                    appointmentDate: availability.Date,
                    appointmentTime: availability.StartTime,
                    sessionType: appointment.SessionType.ToString(),
                    ct: cancellationToken);

                notificationLog.Status = AppointmentNotificationStatus.Sent;
                notificationLog.SentAtUtc = DateTime.UtcNow;
                notificationLog.ErrorMessage = null;

                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                notificationLog.Status = AppointmentNotificationStatus.Failed;
                notificationLog.ErrorMessage = ex.Message;

                await context.SaveChangesAsync(cancellationToken);

                logger.LogError(ex,
                                "Failed to send cancellation email for appointment {AppointmentId} and client {ClientId}",
                                appointment.Id, client.Id);
            }
        }
    }
}
