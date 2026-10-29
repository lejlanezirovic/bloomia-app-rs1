namespace Bloomia.Domain.Common;

public static class BusinessTime
{
    public static readonly TimeZoneInfo Zone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

    public static DateTime Now => FromUtc(DateTime.UtcNow);

    public static DateOnly Today => DateOnly.FromDateTime(Now);

    public static DateTime ToUtc(DateOnly date, TimeOnly time)
        => ToUtc(date.ToDateTime(time));

    public static DateTime ToUtc(DateTime local)
        => TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(local, DateTimeKind.Unspecified), Zone);

    public static DateTime FromUtc(DateTime utc)
        => TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), Zone);
}
