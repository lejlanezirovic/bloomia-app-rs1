using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Commands.Update.UpdateTherapistVerification
{
    public class UpdateTherapisVerificationCommandHandler(IAppDbContext context) : IRequestHandler<UpdateTherapisVerificationCommand>
    {
        public async Task Handle(UpdateTherapisVerificationCommand request, CancellationToken ct)
        {
            var therapist = await context.Therapists
                .FirstOrDefaultAsync(x => x.Id == request.TherapistId, ct);

            if (therapist == null)
                throw new BloomiaNotFoundException("Therapist not found.");

            therapist.IsVerified = request.IsVerified;

            await context.SaveChangesAsync(ct);
        }
    }
}
