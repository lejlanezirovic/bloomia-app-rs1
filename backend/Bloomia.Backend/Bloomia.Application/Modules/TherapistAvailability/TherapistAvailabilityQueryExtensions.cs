using Bloomia.Domain.Common;
using Bloomia.Domain.Entities.TherapistRelated;

namespace Bloomia.Application.Modules.TherapistAvailability
{
    public static class TherapistAvailabilityQueryExtensions
    {
        public static IQueryable<TherapistAvailabilityEntity> WhereOverlapping(
            this IQueryable<TherapistAvailabilityEntity> query, DateOnly date, TimeOnly startTime)
        {
            var earliestConflictingStart = startTime.Add(-SessionRules.Duration);
            var latestConflictingStart = startTime.Add(SessionRules.Duration);

            return query.Where(x =>
                x.Date == date &&
                x.StartTime > earliestConflictingStart &&
                x.StartTime < latestConflictingStart);
        }
    }
}
