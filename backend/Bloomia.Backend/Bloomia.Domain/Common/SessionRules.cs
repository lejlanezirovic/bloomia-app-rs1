namespace Bloomia.Domain.Common;

/// <summary>
/// Business rules shared by every module that works with therapy sessions.
/// </summary>
public static class SessionRules
{
    /// <summary>
    /// Length of a single therapy session. An appointment occupies the interval
    /// [ScheduledAtUtc, ScheduledAtUtc + Duration) and counts as completed once it has ended.
    /// </summary>
    public static readonly TimeSpan Duration = TimeSpan.FromHours(1);
}
