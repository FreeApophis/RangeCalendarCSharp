using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Calendar
{
    internal class MonthLayouter
    {
        private const int DayWidth = 3;
        const int DaysInAWeek = 7;
        const string MonthFormat = "MMMM";

        public static IEnumerable<string> DefaultLayout(IEnumerable<DateTime> month)
            => ImmutableList
                .Create<string>()
                .Add(MonthName(month))
                .Add(WeekDayLine())
                .AddRange(month.GroupBy(d => GetWeekOfYear(d)).Select(FormatWeek))
                .Add($"{string.Empty,DayWidth * DaysInAWeek}");

        private static string MonthName(IEnumerable<DateTime> month)
            => month
                .Select(d => d.ToString(MonthFormat))
                .First()
                .Center(DayWidth * DaysInAWeek);

        private static string FormatWeek(IGrouping<object, DateTime> week)
            => $"{AggregateWeek(week),DayWidth * DaysInAWeek}";

        private static StringBuilder AggregateWeek(IGrouping<object, DateTime> week)
            => week
                .Select(DayOfTheMonth)
                .Aggregate(new StringBuilder(Indent(week)), AggregateDays);

        private static string DayOfTheMonth(DateTime day)
            => $"{day.Day}";

        private static string Indent(IGrouping<object, DateTime> week)
            => new string(' ', DayWidth * MissingFrontDays(week));

        private static int MissingFrontDays(IGrouping<object, DateTime> week)
            => NthDayOfWeek(week.First().DayOfWeek);

        private static string WeekDayLine()
            => WeekDays()
                .OrderBy(NthDayOfWeek)
                .Select(ToShortestDayName)
                .Aggregate(new StringBuilder(), AggregateDays)
                .ToString();

        private static StringBuilder AggregateDays(StringBuilder aggregate, string day)
            => aggregate.Append($"{day,DayWidth}");

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
    }
}