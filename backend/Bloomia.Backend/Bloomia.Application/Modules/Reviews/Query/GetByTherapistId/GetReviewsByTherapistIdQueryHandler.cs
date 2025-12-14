using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Reviews.Query.GetByTherapistId
{
    public sealed class GetReviewsByTherapistIdQueryHandler(IAppDbContext context) :
        IRequestHandler<GetReviewsByTherapistIdQuery, PageResult<GetReviewsByTherapistIdQueryDto>>
    {
        public async Task<PageResult<GetReviewsByTherapistIdQueryDto>> Handle(GetReviewsByTherapistIdQuery request, CancellationToken ct)
        {
            var therapistExists = await context.Therapists
                .AnyAsync(x => x.Id == request.TherapistId && x.isVerified, ct);

            if (!therapistExists)
                throw new BloomiaNotFoundException("Therapist not found.");

            var query = context.Reviews
                .Where(x => x.Appointment.TherapistAvailability.TherapistId == request.TherapistId)
                .OrderByDescending(x => x.CreatedAtUtc);

            var projectedQuery = query
                .Select(x => new GetReviewsByTherapistIdQueryDto
                {
                    Id = x.Id,
                    Rating = x.Rating,
                    Comment = x.Comment,
                    CreatedAt = x.CreatedAtUtc,
                    ClientInitials = x.Appointment.Client.User.Firstname.Substring(0, 1) + ". " +
                                    x.Appointment.Client.User.Lastname.Substring(0, 1) + "."
                });

            return await PageResult<GetReviewsByTherapistIdQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
        }
    }
}
