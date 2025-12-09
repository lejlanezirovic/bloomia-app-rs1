using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Command.Create
{
    public class CreateAppointmentCommandHandler(IAppDbContext context) : IRequestHandler<CreateAppoinmentCommand, CreateAppointmentCommandDto>
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
           
            if(availableTime.Date<DateOnly.FromDateTime(DateTime.UtcNow) || availableTime.Date==DateOnly.FromDateTime(DateTime.UtcNow)&& availableTime.StartTime<=TimeOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new BloomiaConflictException("You can not book an appointment in the past!");
            }

            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            
            if (availableTime.IsBooked) { 
                throw new BloomiaConflictException("That time is already booked!");
            }

            var appointment = new AppointmentEntity
            {
                ClientId = client.Id,
                Client = client,
                BookedAt = DateTime.UtcNow,
                TherapistAvailabilityId = availableTime.Id,
                TherapistAvailability = availableTime,
                SessionType = request.SessionType,
                CreatedAtUtc = DateTime.UtcNow
            };
            context.Appointments.Add(appointment);

            availableTime.IsBooked = true;
            availableTime.Appointment = appointment;

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
    }
}
