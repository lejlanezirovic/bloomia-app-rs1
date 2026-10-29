using Bloomia.Domain.Common;
using Bloomia.Domain.Entities.TherapistRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Command.Create
{
    public class CreateTherapistAvailabilityCommandHandler(IAppDbContext context) : IRequestHandler<CreateTherapistAvailabilityCommand, CreateTherapistAvailabilityCommandDto>
    {
        public async Task<CreateTherapistAvailabilityCommandDto> Handle(CreateTherapistAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var therapist = await context.Therapists.Include(x => x.User).Where(x => x.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null) {
                throw new BloomiaNotFoundException(message: "Therapist not found try to login!");
            }

            if (BusinessTime.ToUtc(request.AvailableDate, request.StartTime) <= DateTime.UtcNow)
                throw new BloomiaConflictException("You cannot add a time slot that has already passed.");

            var overlapsExistingSlot = await context.TherapistAvailabilities
                .Where(x => x.TherapistId == therapist.Id)
                .WhereOverlapping(request.AvailableDate, request.StartTime)
                .AnyAsync(cancellationToken);

            if (overlapsExistingSlot)
            {
                throw new BloomiaConflictException(
                    $"This time overlaps with an existing slot. Sessions last {SessionRules.Duration.TotalMinutes} minutes.");
            }

            
            var newAppointment = new TherapistAvailabilityEntity
            {
                TherapistId = therapist.Id,
                Therapist = therapist,
                Date = request.AvailableDate,
                StartTime = request.StartTime,
                IsBooked = false,
                CreatedAtUtc = DateTime.UtcNow
            };
           context.TherapistAvailabilities.Add(newAppointment);
           await context.SaveChangesAsync(cancellationToken);

            var dto = new CreateTherapistAvailabilityCommandDto
            {
                Note = "You have successfully added an appointment!",
                Date = newAppointment.Date,
                Time = newAppointment.StartTime
            };
            return dto;
        }
    }
}
