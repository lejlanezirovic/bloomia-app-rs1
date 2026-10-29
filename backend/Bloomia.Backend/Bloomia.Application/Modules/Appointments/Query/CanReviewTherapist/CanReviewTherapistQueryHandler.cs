using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Query.CanReviewTherapist
{
    public class CanReviewTherapistQueryHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<CanReviewTherapistQuery, bool>
    {
        public async Task<bool> Handle(CanReviewTherapistQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAuthenticated || !currentUser.IsClient)
                return false;

            var clientId = await context.Clients
                .Where(x => x.UserId == currentUser.UserId)
                .Select(x => (int?)x.Id).FirstOrDefaultAsync(ct);

            if (clientId == null)
                return false;

            var alreadyReviewed = await context.Reviews
                .AnyAsync(x => x.ClientId == clientId.Value && x.TherapistId == request.TherapistId, ct);

            if (alreadyReviewed)
                return false;

            return await context.Appointments
                            .WhereCompleted(DateTime.UtcNow)
                            .AnyAsync(x =>
                                x.ClientId == clientId.Value &&
                                x.TherapistAvailability.TherapistId == request.TherapistId,
                                ct);
        }
    }
}
