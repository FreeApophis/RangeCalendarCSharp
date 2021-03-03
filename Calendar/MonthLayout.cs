using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Calendar
{
    internal class MonthLayout
    {
        private static readonly CalendarWeekRule CalendarWeekRule = CurrentDateTimeFormat.CalendarWeekRule;
        private static readonly DayOfWeek FirstDayOfWeek = CurrentDateTimeFormat.FirstDayOfWeek;
        private const int RowSeparationSpace = 1;
        private const int MinDayWidth = 3;
        private const int DaysInAWeek = 7;
        private const char SpaceCharacter = ' ';
        private const string MonthFormat = "MMMM yyyy";

        public static Func<IEnumerable<DateTime>, IEnumerable<string>> Default(Func<IEnumerable<DateTime>, string> monthName, Func<string> weekDayLine, Func<IEnumerable<DateTime>, IEnumerable<string>> weeksOfMonth)
        => month
            => DefaultLayout(monthName, weekDayLine, weeksOfMonth)(month.ToImmutableList());
        public static Func<IList<DateTime>, IEnumerable<string>> DefaultLayout(Func<IEnumerable<DateTime>, string> monthName, Func<string> weekDayLine, Func<IEnumerable<DateTime>, IEnumerable<string>> weeksOfMonth)
            => month
                => ImmutableList<string>
                    .Empty
                    .Add(monthName(month))
                    .Add(weekDayLine())
                    .AddRange(weeksOfMonth(month))
                    .Add(Spaces(WidthOfWeek()));

        public static Func<IEnumerable<DateTime>, IEnumerable<string>> WeeksOfMonth(Func<IGrouping<object, DateTime>, string> formatWeek)
            => month
                => month
                    .GroupBy(ByWeekOfYear)
                    .Select(formatWeek);

        private static object ByWeekOfYear(DateTime day)
            => GetWeekOfYear(day);

        public static Func<IEnumerable<DateTime>, string> MonthName(ColorizeString colorizeString)
            => month
                => colorizeString(CenterMonth(month), Color.White);

        private static string CenterMonth(IEnumerable<DateTime> month)
        {
            return month
                .Select(MonthString)
                .First()
                .Center(WidthOfWeek());
        }

        private static string MonthString(DateTime day)
            => day
                .ToString(MonthFormat);

        public static Func<IGrouping<object, DateTime>, string> FormatWeek(Func<IEnumerable<DateTime>, string> aggregateWeek)
            => week
                => PadWeek(aggregateWeek(week), week);

        public static Func<IEnumerable<DateTime>, string> AggregateWeek(Func<StringBuilder, DateTime, StringBuilder> aggregateDays)
            => week
                => week
                    .Aggregate(new StringBuilder(), aggregateDays)
                    .ToString();

        public static Func<StringBuilder, DateTime, StringBuilder> AggregateDays(Func<DateTime, string> formatDay)
            => (aggregate, day)
                => aggregate.Append(formatDay(day));

        public static Func<DateTime, string> FormatDay(ColorizeString colorizeString, ColorizeString colorizeStringBg)
            => day
                 => colorizeStringBg(colorizeString(PadDay(day), ColorService.WeekDayColor(day.DayOfWeek)), ColorService.DayColor(day));

        private static string PadDay(DateTime day)
        {
            return day
                .Day
                .ToString()
                .PadLeft(WidthOfWeekDay(day.DayOfWeek));
        }

        public static Func<string> WeekDayLine(Func<StringBuilder, DayOfWeek, StringBuilder> aggregateWeekDays)
            => ()
                => WeekDays()
                    .OrderBy(NthDayOfWeek)
                    .Aggregate(new StringBuilder(), aggregateWeekDays)
                    .ToString();

        public static Func<StringBuilder, DayOfWeek, StringBuilder> AggregateWeekDays(Func<DayOfWeek, string> formattedWeekDay)
            => (aggregate, day)
                => aggregate.Append(formattedWeekDay(day));

        public static Func<DayOfWeek, string> FormattedWeekDay(ColorizeString colorizeString)
            => day
                => colorizeString(ToShortestDayName(day)
                    .PadLeft(WidthOfWeekDay(day)), ColorService.WeekDayColor(day));

        private static IEnumerable<DayOfWeek> WeekDays()
            => Enum
                .GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>();

        private static int NthDayOfWeek(DayOfWeek dayOfWeek)
            => (dayOfWeek + DaysInAWeek - FirstDayOfWeek) % DaysInAWeek;

        private static string ToShortestDayName(DayOfWeek dayOfWeek)
            => CurrentDateTimeFormat
                .GetShortestDayName(dayOfWeek);

        private static object GetWeekOfYear(in DateTime dateTime)
            => CultureInfo
                .CurrentCulture
                .Calendar
                .GetWeekOfYear(dateTime, CalendarWeekRule, FirstDayOfWeek);

        private static DateTimeFormatInfo CurrentDateTimeFormat
            => CultureInfo.CurrentCulture.DateTimeFormat;

        private static int WidthOfWeek()
            => WeekDays()
                .Sum(WidthOfWeekDay);

        private static int WidthOfWeekDay(DayOfWeek day)
            => Math
                .Max(ColumnWidth(day), MinDayWidth);

        private static int ColumnWidth(DayOfWeek day)
            => ToShortestDayName(day).Length
               + RowSeparationSpace;

        private static string Spaces(int width)
            => new(SpaceCharacter, width);

        private static string PadWeek(string formattedWeek, IGrouping<object, DateTime> week)
            => StartsOnFirstDayOfWeek(week)
                ? formattedWeek + Spaces(EndOfWeekSpaces(week.Count()))
                : Spaces(BeginOfWeekSpaces(DaysInAWeek - week.Count())) + formattedWeek;

        private static int EndOfWeekSpaces(int skipDays)
            => WeekDays()
                .OrderBy(NthDayOfWeek)
                .Skip(skipDays)
                .Sum(WidthOfWeekDay);

        private static int BeginOfWeekSpaces(int takeDays)
            => WeekDays()
                .OrderBy(NthDayOfWeek)
                .Take(takeDays)
                .Sum(WidthOfWeekDay);

        private static bool StartsOnFirstDayOfWeek(IEnumerable<DateTime> week)
            => NthDayOfWeek(week.First().DayOfWeek) == 0;
    }
}