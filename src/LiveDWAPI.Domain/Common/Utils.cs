namespace LiveDWAPI.Domain.Common;

public static class Utils
{
    public static DateTime GetLastReportingMonth ()
    {
        var lastMonth = DateTime.Now.AddMonths(-1);
        return new DateTime(lastMonth.Year, lastMonth.Month, 1).Date;
    }
}