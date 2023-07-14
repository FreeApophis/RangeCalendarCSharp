using System.Drawing;
using Nager.Date;
using Nager.Date.Model;

namespace Calendar;

internal static class ColorService
{
    static ColorService()
        => DateSystem.LicenseKey = "LostTimeIsNeverFoundAgain";

    public static Color WeekDayColor(DayOfWeek day)
        => IsWeekend(day)
            ? Color.OrangeRed
            : Color.LightGray;

    public static Color DayColor(DateOnly day)
        => IsHoliday(day)
            ? Color.FromArgb(127, 0, 0)
            : Color.Transparent;

    private static bool IsWeekend(DayOfWeek day)
        => DateSystem
            .GetWeekendProvider(ConsoleArguments.CountryFromCulture())
            .IsWeekend(day);

    private static bool IsHoliday(DateOnly day)
        => DateSystem
            .GetPublicHolidayProvider(ConsoleArguments.CountryFromCulture())

            .Get(day.Year)
            .Any(IsSameDay(day));

    private static Func<PublicHoliday, bool> IsSameDay(DateOnly day)
        => holiday
            => holiday.Date.DayOfYear == day.DayOfYear;
}
