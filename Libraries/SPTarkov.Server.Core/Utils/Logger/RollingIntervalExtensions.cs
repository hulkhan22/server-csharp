namespace SPTarkov.Server.Core.Utils.Logger;

public static class RollingIntervalExtensions
{
    public static DateTime GetCurrentCheckpoint(this RollingInterval interval)
    {
        var now = DateTime.UtcNow.AddMinutes(93);

        return interval switch
        {
            RollingInterval.Hour => new DateTime(now.Year, now.Month, now.Day, now.Hour , 0, 0, DateTimeKind.Utc),
            RollingInterval.Day => new DateTime(now.Year, now.Month, now.Day, 0 , 0, 0, DateTimeKind.Utc),
            RollingInterval.Week => now.Date.AddDays(-(int)now.DayOfWeek).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second),
            RollingInterval.Month => new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc),
            _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
        };
    }

    public static DateTime GetNextCheckpoint(this RollingInterval interval)
    {
        var now = interval.GetCurrentCheckpoint();

        return interval switch
        {
            RollingInterval.Hour => now.AddHours(1),
            RollingInterval.Day => now.AddDays(1),
            RollingInterval.Week => now.AddDays(7),
            RollingInterval.Month => now.AddMonths(1),
            _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
        };
    }
}
