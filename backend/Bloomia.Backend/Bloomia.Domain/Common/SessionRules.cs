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

    public static readonly TimeOnly WorkdayStart = new(8, 0);

    public static readonly TimeOnly WorkdayEnd = new(17, 0);

    public static TimeOnly LatestStartTime => WorkdayEnd.Add(-Duration);

    public static bool IsWithinWorkingHours(TimeOnly start)
        => start >= WorkdayStart && start <= LatestStartTime;

    public static string WorkingHoursMessage =>
        $"Sessions must take place within business hours ({WorkdayStart:HH\\:mm} - {WorkdayEnd:HH\\:mm}); the latest start time is {LatestStartTime:HH\\:mm}.";
}
