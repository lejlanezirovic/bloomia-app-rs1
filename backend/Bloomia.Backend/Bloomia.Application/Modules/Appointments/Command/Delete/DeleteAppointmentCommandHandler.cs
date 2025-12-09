using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Command.Delete
{
    public class DeleteAppointmentCommandHandler(IAppDbContext context) : IRequestHandler<DeleteAppointmentCommand, string>
    {
        public async Task<string> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            //ovo moze obrisati klijent ili terapeut
            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            var therapist = await context.Therapists.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);

            var appointment = await context.Appointments.Include(x=>x.TherapistAvailability)
                .ThenInclude(x=>x.Therapist).FirstOrDefaultAsync(x => x.Id == request.AppointmentId, cancellationToken);

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

            await using var transaction=await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                appointment.IsDeleted = true;
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

            return "Appointment deleted successfully";
        }
    }
}
