using System.Globalization;

namespace Homie.Extensions;

public static class MathExtensions
{
    public static DateTime Max(this DateTime d1, DateTime? d2) => d2 is null ? d1 : (d1 > d2.Value ? d1 : d2.Value);
    public static DateTime Min(this DateTime d1, DateTime? d2) => d2 is null ? d1 : (d1 <= d2.Value ? d1 : d2.Value);

    public static bool IsBetween(this DateTime d1, DateTime? start, DateTime? end) 
        => (start is not null && end is not null) && (d1 >= start.Value && d1 <= end.Value);
    
    public static DateTime LastSunday(this DateTime dateTime, int weeks = 0) => dateTime.AddDays(DayOfWeek.Sunday - dateTime.DayOfWeek - 7 * weeks);

    public static DateTime NextSaturday(this DateTime dateTime, int weeks = 0) => dateTime.LastSunday(-weeks).AddDays(6);

    public static int WeekNumber(this DateTime dateTime)
    {
        var weekday = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dateTime);

        int correction = weekday < DayOfWeek.Wednesday ? 3 : 0;

        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dateTime.AddDays(correction),
            CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
    }

    public static DateTime FirstSundayOfMonth(this DateTime dateTime)
    {
        var adjusted = dateTime.LastSunday().AddDays(3);
        return new DateTime(adjusted.Year, adjusted.Month, 1).AddDays(3).LastSunday();
    }
    
    public static DateTime LastSaturdayOfMonth(this DateTime dateTime)
    {
        var adjusted = dateTime.NextSaturday().AddDays(-3);
        return new DateTime(adjusted.Year, adjusted.Month, DateTime.DaysInMonth(adjusted.Year, adjusted.Month)).AddDays(-3).NextSaturday();
    }
}