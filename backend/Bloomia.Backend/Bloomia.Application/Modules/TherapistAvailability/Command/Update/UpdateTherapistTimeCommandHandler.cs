using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Command.Update
{
    public class UpdateTherapistTimeCommandHandler(IAppDbContext context) : IRequestHandler<UpdateTherapistTimeCommand, UpdateTherapistTimeCommandDto>
    {
        public async Task<UpdateTherapistTimeCommandDto> Handle(UpdateTherapistTimeCommand request, CancellationToken cancellationToken)
        {
            var therapist = await context.Therapists.Include(x => x.User).Where(x => x.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                throw new BloomiaNotFoundException(message: "Therapist not found try to login!");
            }
            var therapistAvailability = await context.TherapistAvailabilities.Include(x=>x.Therapist).ThenInclude(x=>x.User)
                    .Where(x => x.TherapistId == therapist.Id && x.Id == request.TherapistAvailabilityId && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);

            if (therapistAvailability == null) {

                throw new BloomiaNotFoundException(message: "Selected availability slot does not exist or does not belong to this therapist.");
            }
            if (therapistAvailability.IsBooked == true || therapistAvailability.AppointmentId > 0)
            {
                throw new BloomiaConflictException("This slot cannot be updated because it is already booked");
            }
            var existingAvailability =await context.TherapistAvailabilities
                    .AnyAsync(x => x.TherapistId == therapist.Id && x.Date == request.NewDate
                            && x.StartTime == request.NewTime && x.Id!=therapistAvailability.Id && !x.IsDeleted
                            , cancellationToken);
           
            if (existingAvailability==true)
            {
                throw new BloomiaConflictException("The new date and time overlap with an existing availability or booked appointment.");
            }
            var dto = new UpdateTherapistTimeCommandDto
            {
                TherapistAvailabilityId = therapistAvailability.Id,
                OldDate = therapistAvailability.Date,
                OldTime = therapistAvailability.StartTime
            };
            therapistAvailability.Date = request.NewDate;
            therapistAvailability.StartTime = request.NewTime;
            therapistAvailability.ModifiedAtUtc= DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);

            dto.NewDate = therapistAvailability.Date;
            dto.NewTime = therapistAvailability.StartTime;
            return dto;
        }
    }
}
