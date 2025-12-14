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

            var appointment = await context.Appointments
                .Include(x => x.Client)
                .Include(x => x.TherapistAvailability)
                    .ThenInclude(ta => ta.Therapist)
                .FirstOrDefaultAsync(x => x.Id == request.AppointmentId
                    && x.Client.UserId == currentUser.UserId, ct);

            if (appointment == null)
                throw new BloomiaBusinessRuleException("", "You don't have an appointment with this AppointmentId");

            var reviewExists = await context.Reviews
                .AnyAsync(x => x.AppointmentId == appointment.Id, ct);

            if(reviewExists)
                throw new BloomiaBusinessRuleException("", "You have already reviewed this appointment.");

            var appointmentEndTimeUtc = appointment.ScheduledAtUtc.AddHours(1);
           
            if (appointmentEndTimeUtc >= DateTime.UtcNow)
                throw new BloomiaBusinessRuleException("", "You can't review appointments which have not finished yet.");

            var review = new ReviewEntity
            {
                AppointmentId = request.AppointmentId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAtUtc = DateTime.UtcNow,
            };

            await context.Reviews.AddAsync(review);
            await context.SaveChangesAsync(ct);

            var therapistId = appointment.TherapistAvailability.TherapistId;

            var therapist = await context.Therapists
                .FirstOrDefaultAsync(x => x.Id == therapistId, ct);

            if (therapist == null)
                throw new BloomiaNotFoundException("Therapist not found");

            var averageRating = await context.Reviews
                .Where(x => x.Appointment.TherapistAvailability.TherapistId == therapistId)
                .AverageAsync(x => (float)x.Rating, ct);

            therapist.RatingAvg = averageRating;
            await context.SaveChangesAsync(ct);

            return review.Id;

        }
    }
}
