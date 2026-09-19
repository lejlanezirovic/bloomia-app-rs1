using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Entities.ReviewsFolder;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Bloomia.Application.Modules.Reviews.Commands.Create
{
    public sealed class CreateReviewCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<CreateReviewCommand, int>
    {
        public async Task<int> Handle(CreateReviewCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAuthenticated)
                throw new BloomiaBusinessRuleException("NOT_LOGGED_IN", "You have to be logged in.");

            if(!currentUser.IsClient)
                throw new BloomiaBusinessRuleException("USER_NOT_AUTH", "Only clients can leave reviews.");

            var client = await context.Clients
                .FirstOrDefaultAsync(x => x.UserId == currentUser.UserId, ct);

            if (client == null)
                throw new BloomiaNotFoundException("Client not found");

            var therapist = await context.Therapists
                                    .FirstOrDefaultAsync(
                                        x => x.Id == request.TherapistId,
                                        ct);

            if (therapist == null)
                throw new BloomiaNotFoundException("Therapist not found.");

            var hasCompletedAppointment = await context.Appointments
                .AnyAsync(x => x.ClientId == client.Id &&
                                x.TherapistAvailability.TherapistId == request.TherapistId &&
                                x.ScheduledAtUtc.AddHours(1) <= DateTime.UtcNow, ct);

            if (!hasCompletedAppointment)
            {
                throw new BloomiaBusinessRuleException("NO_COMPLETED_APPOINTMENT",
                    "You can leave a review only after completing an appointment with this therapist.");
            }

            var review = new ReviewEntity
            {
                ClientId = client.Id,
                TherapistId = request.TherapistId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAtUtc = DateTime.UtcNow,
            };

            await context.Reviews.AddAsync(review);
            await context.SaveChangesAsync(ct);

            var averageRating = await context.Reviews
                .Where(x => x.TherapistId == therapist.Id)
                .AverageAsync(x => (float)x.Rating, ct);

            therapist.RatingAvg = (float)Math.Round(averageRating, 1);
            await context.SaveChangesAsync(ct);

            return review.Id;

        }
    }
}
