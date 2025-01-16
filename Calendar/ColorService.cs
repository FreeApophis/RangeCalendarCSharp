using System.Drawing;
using Nager.Date;
using Nager.Date.Models;

namespace Calendar;

internal static class ColorService
{
    public static Color WeekDayColor(DayOfWeek day)
        => IsWeekend(day)
            ? Color.OrangeRed
            : Color.LightGray;

    public static Color DayColor(DateOnly day)
        => IsHoliday(day)
            ? Color.FromArgb(127, 0, 0)
            : Color.Transparent;

    private static bool IsWeekend(DayOfWeek day)
        => WeekendSystem
            .GetWeekendProvider(ConsoleArguments.CountryFromCulture())
            .IsWeekend(day);

    private static bool IsHoliday(DateOnly day)
        => HolidaySystem
            .GetHolidayProvider(ConsoleArguments.CountryFromCulture())
            .GetHolidays(day.Year)
            .Any(IsSameDay(day));

    private static Func<Holiday, bool> IsSameDay(DateOnly day)
        => holiday
            => holiday.Date.DayOfYear == day.DayOfYear;
}
