namespace Bloomia.Domain.Entities.Enums
{
    /// <summary>
    /// Lifecycle status of an appointment. Cancelled appointments stay in the database
    /// (they are not soft-deleted), so every query over Appointments must exclude them
    /// explicitly, e.g. via AppointmentQueryExtensions.WhereNotCancelled().
    /// "Completed" is not stored; it is derived from ScheduledAtUtc and SessionRules.Duration.
    /// </summary>
    public enum AppointmentStatus
    {
        Scheduled = 0,
        Cancelled = 1
    }
}
