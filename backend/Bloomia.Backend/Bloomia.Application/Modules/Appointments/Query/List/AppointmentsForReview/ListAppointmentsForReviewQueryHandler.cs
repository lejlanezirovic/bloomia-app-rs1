using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Query.List.AppointmentsForReview
{
    public sealed class ListAppointmentsForReviewQueryHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<ListAppointmentsForReviewQuery, List<ListAppointmentsForReviewQueryDto>>
    {
        public async Task<List<ListAppointmentsForReviewQueryDto>> Handle(ListAppointmentsForReviewQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAuthenticated)
                throw new BloomiaBusinessRuleException("NOT_LOGGED_IN", "You have to be logged in.");

            if (!currentUser.IsClient)
                throw new BloomiaBusinessRuleException("USER_NOT_AUTH", "Only clients can review therapists.");

            var therapistExists = await context.Therapists.AnyAsync(x => x.Id == request.TherapistId, ct);

            if (!therapistExists)
                throw new BloomiaNotFoundException("Therapist not found.");

            var now = DateTime.UtcNow;

            var query = context.Appointments.AsNoTracking().Where(x => x.Client.UserId == currentUser.UserId &&
                        x.TherapistAvailability.TherapistId == request.TherapistId &&
                        x.ScheduledAtUtc.AddHours(1) < now &&
                        !context.Reviews.Any(r => r.AppointmentId == x.Id))
                .OrderByDescending(x => x.ScheduledAtUtc)
                .Select(x => new ListAppointmentsForReviewQueryDto
                {
                    AppointmentId = x.Id,
                    ScheduledAtUtc = x.ScheduledAtUtc
                });

            return await query.ToListAsync(ct);
        }
    }
}
