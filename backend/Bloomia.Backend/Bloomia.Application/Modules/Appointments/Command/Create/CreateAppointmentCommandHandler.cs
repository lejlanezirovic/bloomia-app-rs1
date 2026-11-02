using Bloomia.Domain.Common;
using Bloomia.Domain.Entities;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.Sessions;
using Bloomia.Domain.Entities.TherapistRelated;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Command.Create
{
    public class CreateAppointmentCommandHandler(IAppDbContext context, IEmailService emailService, ILogger<CreateAppointmentCommandHandler> logger) : IRequestHandler<CreateAppoinmentCommand, CreateAppointmentCommandDto>
    {
        public async Task<CreateAppointmentCommandDto> Handle(CreateAppoinmentCommand request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            if (client == null) {
                throw new BloomiaNotFoundException("Client not found!");
            }

            var availableTime = await context.TherapistAvailabilities.Include(x => x.Therapist).ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.TherapistAvailabilityId, cancellationToken);

            if (availableTime==null || availableTime.IsDeleted==true)
            {
                throw new BloomiaNotFoundException("That start time is not found!");
            }

            if (!availableTime.Therapist.IsVerified)
            {
                throw new BloomiaBusinessRuleException("THERAPIST_NOT_VERIFIED", "You can only book appointments with verified therapists.");
            }

            var scheduledUtc = BusinessTime.ToUtc(availableTime.Date, availableTime.StartTime);

            if (scheduledUtc <= DateTime.UtcNow)
            {
                throw new BloomiaConflictException("You can not book an appointment in the past!");
            }

            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            if (availableTime.IsBooked) {
                throw new BloomiaConflictException("That time is already booked!");
            }

            var clientHasOverlappingAppointment = await context.Appointments
                .WhereNotCancelled()
                .WhereOverlapping(scheduledUtc)
                .AnyAsync(x => x.ClientId == client.Id, cancellationToken);

            if (clientHasOverlappingAppointment)
                throw new BloomiaBusinessRuleException("APPOINTMENT", "You already have an appointment that overlaps with this time.");

            var therapistHasOverlappingAppointment = await context.Appointments
                .WhereNotCancelled()
                .WhereOverlapping(scheduledUtc)
                .AnyAsync(x => x.TherapistAvailability.TherapistId == availableTime.TherapistId, cancellationToken);

            if (therapistHasOverlappingAppointment)
                throw new BloomiaConflictException("The therapist already has an appointment that overlaps with this time.");

            var appointment = new AppointmentEntity
            {
                ClientId = client.Id,
                Client = client,
                BookedAt = DateTime.UtcNow,
                TherapistAvailabilityId = availableTime.Id,
                TherapistAvailability = availableTime,
                SessionType = request.SessionType,
                CreatedAtUtc = DateTime.UtcNow,
                ScheduledAtUtc = scheduledUtc
            };
            context.Appointments.Add(appointment);

            availableTime.IsBooked = true;

            var clientEmail = client.User.Email;
            var hasClientEmail = !string.IsNullOrWhiteSpace(clientEmail);

            var notificationLog = new AppointmentNotificationLogEntity
            {
                Appointment = appointment,
                RecipientEmail = clientEmail ?? string.Empty,
                NotificationType = AppointmentNotificationsType.BookingConfirmation,
                Status = hasClientEmail ? AppointmentNotificationStatus.Pending : AppointmentNotificationStatus.Failed,
                ErrorMessage = hasClientEmail ? null : "Client email is missing.",
                CreatedAtUtc = DateTime.UtcNow
            };
            context.AppointmentNotificationLogs.Add(notificationLog);

            try
            {
                 await context.SaveChangesAsync(cancellationToken);
                 await transaction.CommitAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                if(ex.InnerException?.Message.Contains("duplicate")==true ||
                    ex.InnerException?.Message.Contains("unique") == true)
                {
                    throw new BloomiaConflictException("Time was booked by another client a moment ago");
                }
                throw;
            }

            if (hasClientEmail)
            {
                await SendBookingConfirmationAsync(client, availableTime, appointment, notificationLog, cancellationToken);
            }
            else
            {
                logger.LogWarning(
                    "Booking confirmation email skipped for appointment {AppointmentId} because client email is missing.",
                    appointment.Id);
            }

            var dto = new CreateAppointmentCommandDto
            {
                Note = "You have successfully booked an appointment.",
                BookedAt = appointment.BookedAt,
                SessionType = appointment.SessionType.ToString(),
                StartTime = appointment.TherapistAvailability.StartTime,
                Date = appointment.TherapistAvailability.Date,
                TherapistFullname = appointment.TherapistAvailability.Therapist.User.Fullname
            };
            return dto;
        }

        private async Task SendBookingConfirmationAsync(ClientEntity client, TherapistAvailabilityEntity availableTime,
            AppointmentEntity appointment, AppointmentNotificationLogEntity notificationLog, CancellationToken cancellationToken)
        {
            try
            {
                var clientName = client.User.Fullname ?? $"{client.User.Firstname} {client.User.Lastname}";
                var therapistName = availableTime.Therapist.User.Fullname ??
                                    $"{availableTime.Therapist.User.Firstname} {availableTime.Therapist.User.Lastname}";

                await emailService.SendAppointmentBookingConfirmationAsync(
                    toEmail: notificationLog.RecipientEmail,
                    clientName: clientName,
                    therapistName: therapistName,
                    appointmentDate: availableTime.Date,
                    appointmentTime: availableTime.StartTime,
                    sessionType: appointment.SessionType.ToString(),
                    ct: cancellationToken);

                notificationLog.Status = AppointmentNotificationStatus.Sent;
                notificationLog.SentAtUtc = DateTime.UtcNow;
                notificationLog.ErrorMessage = null;
            }
            catch(Exception ex)
            {
                notificationLog.Status = AppointmentNotificationStatus.Failed;
                notificationLog.ErrorMessage = ex.Message;

                logger.LogError(ex,
                                "Failed to send booking confirmation email for appointment {AppointmentId} and client {ClientId}",
                                appointment.Id, client.Id);
            }

            try
            {
                await context.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                                "Failed to save booking confirmation status for appointment {AppointmentId}",
                                appointment.Id);
            }
        }
    }
}
