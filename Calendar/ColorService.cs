using System;
using System.Drawing;
using System.Linq;
using Nager.Date;

namespace Calendar
{
    class ColorService
    {
        public static bool Fancy { get; set; } = false;

        public static Color WeekDayColor(DayOfWeek day)
            => IsWeekend(day)
                ? Color.OrangeRed
                : Color.LightGray;

        public static Color DayColor(in DateTime day)
            => IsHoliday(day)
                ? Color.FromArgb(127, 0, 0)
                : Color.Transparent;

        private static bool IsWeekend(DayOfWeek day)
            => DateSystem
                .GetWeekendProvider(ConsoleArguments.CountryFromCulture())
                .IsWeekend(day);

        private static bool IsHoliday(DateTime day)
            => DateSystem
                .GetPublicHolidayProvider(ConsoleArguments.CountryFromCulture())
                .Get(day.Year)
                .Any(d => d.Date.DayOfYear == day.DayOfYear);
    }
}
