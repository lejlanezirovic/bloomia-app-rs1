using Bloomia.Domain.Common;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.Sessions;

namespace Bloomia.Application.Modules.Appointments
{
    /// <summary>
    /// Single definition of "active" and "completed" appointments, shared by all modules.
    /// </summary>
    public static class AppointmentQueryExtensions
    {
        public static IQueryable<AppointmentEntity> WhereNotCancelled(this IQueryable<AppointmentEntity> query)
            => query.Where(x => x.Status != AppointmentStatus.Cancelled);

        /// <summary>
        /// Appointments that were not cancelled and whose session has already ended.
        /// </summary>
        public static IQueryable<AppointmentEntity> WhereCompleted(this IQueryable<AppointmentEntity> query, DateTime nowUtc)
        {
            var endedBefore = CompletedStartBoundary(nowUtc);
            return query.Where(x => x.Status != AppointmentStatus.Cancelled && x.ScheduledAtUtc <= endedBefore);
        }

        public static bool IsCompleted(this AppointmentEntity appointment, DateTime nowUtc)
            => appointment.Status != AppointmentStatus.Cancelled &&
               appointment.ScheduledAtUtc <= CompletedStartBoundary(nowUtc);

        // An appointment that started at or before this moment has already ended.
        public static DateTime CompletedStartBoundary(DateTime nowUtc) => nowUtc - SessionRules.Duration;
    }
}
