using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Command.Delete.DeleteTimeByDate
{
    public class DeleteTherapistAvailableTimeByDateCommandHandler(IAppDbContext context) : IRequestHandler<DeleteTherapistAvailableTimeByDateCommand, string>
    {
        public async Task<string> Handle(DeleteTherapistAvailableTimeByDateCommand request, CancellationToken cancellationToken)
        {
            var therapist = await context.Therapists.Include(x => x.User).Where(x => x.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                throw new BloomiaNotFoundException(message: "Therapist not found try to login!");
            }

            var therapistTimes = await context.TherapistAvailabilities.Include(x => x.Therapist)
                    .Where(x => x.TherapistId == therapist.Id && x.Date == request.Date && x.StartTime == request.TimeToDelete && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);

            if (therapistTimes == null) {
                throw new BloomiaNotFoundException($"You don't have an appointment at {request.TimeToDelete}h  for {request.Date}!");
            }
            if(therapistTimes.IsBooked== true)
            {
                throw new BloomiaConflictException(message: "You can not delete a booked appointment!");
            }

            therapistTimes.IsDeleted = true;
            context.TherapistAvailabilities.Update(therapistTimes);
            await context.SaveChangesAsync(cancellationToken);
            return "You have successfully deleted the time slot from your availability!";
        }
    }
}
