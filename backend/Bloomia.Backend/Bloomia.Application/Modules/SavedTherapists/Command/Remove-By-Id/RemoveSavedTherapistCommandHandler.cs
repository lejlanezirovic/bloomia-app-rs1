using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Command.Remove
{
    public class RemoveSavedTherapistCommandHandler(IAppDbContext context) : IRequestHandler<RemoveSavedTherapistCommand, string>
    {
        public async Task<string> Handle(RemoveSavedTherapistCommand request, CancellationToken cancellationToken)
        {
            //radimo soft delete za soft delete potreban je apply global filter sto mi imamo u 
            //bez naglog brisanja podataka iz baze samo kao prekidac iskljucimo vezu /postojanje izmedju korisnika i terapeuta
            var client = await context.Clients.Include(x => x.User).Where(x => x.User.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (client == null)
            {
                throw new BloomiaNotFoundException("Client not found");
            }
            var therapist = await context.Therapists.Include(x => x.User).Where(x => x.Id == request.TherapistId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                throw new BloomiaNotFoundException("Therapist not found");
            }
            var savedTherapist = await context.SavedTherapists.Include(x => x.Client).Include(x => x.Therapist)
                    .Where(x => x.Client.Id == client.Id && x.Therapist.Id == therapist.Id).FirstOrDefaultAsync(cancellationToken);

            if (savedTherapist == null || savedTherapist.IsDeleted==true)
            {
                throw new BloomiaNotFoundException("Terapeut nije pronadjen u listi");
            }
            if (savedTherapist.IsDeleted == false)
            {
                savedTherapist.IsDeleted = true;
                savedTherapist.ModifiedAtUtc = DateTime.UtcNow;
                await context.SaveChangesAsync(cancellationToken);
            }
            return $"Therapist with id {therapist.Id} removed from saved list successfully";
        }
    }
}
