using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Calendar
{
    internal class MonthLayouter
    {
        private const int RowSeparationSpace = 1;
        private const int MinDayWidth = 3;
        private const int DaysInAWeek = 7;
        private const char SpaceCharacter = ' ';
        const string MonthFormat = "MMMM";

        public static IEnumerable<string> DefaultLayout(IEnumerable<DateTime> month)
            => ImmutableList
                .Create<string>()
                .Add(MonthName(month))
                .Add(WeekDayLine())
                .AddRange(month.GroupBy(d => GetWeekOfYear(d)).Select(FormatWeek))
                .Add(Spaces(WidthOfWeek()));

        private static string MonthName(IEnumerable<DateTime> month)
            => month
                .Select(d => d.ToString(MonthFormat))
                .First()
                .Center(WidthOfWeek());

        private static string FormatWeek(IGrouping<object, DateTime> week)
            => PadWeek(AggregateWeek(week), week);

        private static string AggregateWeek(IGrouping<object, DateTime> week)
            => week
                .Aggregate(new StringBuilder(), AggregateDays)
                .ToString();

        private static StringBuilder AggregateDays(StringBuilder aggregate, DateTime day)
            => aggregate.Append(FormatDay(day));

        private static string FormatDay(DateTime day)
            => day
                .Day
                .ToString()
                .PadLeft(WidthOfWeekDay(day.DayOfWeek));

        private static string WeekDayLine()
            => WeekDays()
                .OrderBy(NthDayOfWeek)
                .Aggregate(new StringBuilder(), AggregateWeekDays)
                .ToString();

        private static StringBuilder AggregateWeekDays(StringBuilder aggregate, DayOfWeek day)
            => aggregate.Append(FormattedWeekDay(day));

        private static string FormattedWeekDay(DayOfWeek day)
            => ToShortestDayName(day)
                .PadLeft(WidthOfWeekDay(day));

        private static IEnumerable<DayOfWeek> WeekDays()
            => Enum
                .GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>();

        private static int NthDayOfWeek(DayOfWeek dayOfWeek)
            => (dayOfWeek + DaysInAWeek - CurrentDateTimeFormat.FirstDayOfWeek) % DaysInAWeek;

        private static string ToShortestDayName(DayOfWeek dayOfWeek)
            => CurrentDateTimeFormat.GetShortestDayName(dayOfWeek);

        private static object GetWeekOfYear(in DateTime dateTime)
            => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CurrentDateTimeFormat.CalendarWeekRule, CurrentDateTimeFormat.FirstDayOfWeek);

        private static DateTimeFormatInfo CurrentDateTimeFormat
            => CultureInfo.CurrentCulture.DateTimeFormat;

        private static int WidthOfWeek()
            => WeekDays()
                .Sum(WidthOfWeekDay);

        private static int WidthOfWeekDay(DayOfWeek day) =>
            Math.Max(ToShortestDayName(day).Length + RowSeparationSpace, MinDayWidth);

        private static string Spaces(int width)
            => new string(SpaceCharacter, width);

        private static string PadWeek(string formattedWeek, IGrouping<object, DateTime> week)
            => StartsOnFirstDayOfWeek(week)
                ? formattedWeek.PadRight(WidthOfWeek())
                : formattedWeek.PadLeft(WidthOfWeek());

        private static bool StartsOnFirstDayOfWeek(IGrouping<object, DateTime> week)
            => NthDayOfWeek(week.First().DayOfWeek) == 0;
    }
}