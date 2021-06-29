using Funcky.Monads;
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
        const string MonthFormat = "MMMM yyyy";

        public static Reader<Enviroment, IEnumerable<string>> DefaultLayout(IEnumerable<DateTime> month)
                => from colorizedMonthName in ColorizedMonthName(month)
                   from weekDayLine in WeekDayLine()
                   from weeksInMonth in month
                    .GroupBy(GetWeekOfYear)
                    .Select(FormatWeek)
                    .Sequence()
                   select BuildDefaultLayout(colorizedMonthName, weekDayLine, weeksInMonth);

        private static ImmutableList<string> BuildDefaultLayout(string colorizedMonthName, string weekDayLine, IEnumerable<string> weeks)
            => ImmutableList
                .Create<string>()
                .Add(colorizedMonthName)
                .Add(weekDayLine)
                .AddRange(weeks)
                .Add(Spaces(WidthOfWeek()));

        private static Reader<Enviroment, string> ColorizedMonthName(IEnumerable<DateTime> month)
            => MonthName(month)
                .Colorize(Color.White);

        private static string MonthName(IEnumerable<DateTime> month)
            => month
                .Select(FormatMonthName)
                .First()
                .Center(WidthOfWeek());

        private static string FormatMonthName(DateTime month)
            => month
                .ToString(MonthFormat);

        private static Reader<Enviroment, string> FormatWeek(IGrouping<int, DateTime> week)
            => from aggregateWeek in AggregateWeek(week)
               select PadWeek(aggregateWeek, week);

        private static Reader<Enviroment, string> AggregateWeek(IGrouping<int, DateTime> week)
            => from formatDay in week
                .Select(FormatDay)
                .Sequence()
               select formatDay
                .Aggregate(new StringBuilder(), AggregateString)
                .ToString();

        private static StringBuilder AggregateString(StringBuilder aggregate, string formattedString)
            => aggregate
                .Append(formattedString);

        private static Reader<Enviroment, string> FormatDay(DateTime day)
            => from colorized in day
                .Day
                .ToString()
                .PadLeft(WidthOfWeekDay(day.DayOfWeek))
                .Colorize(ColorService.WeekDayColor(day.DayOfWeek))
               from background in colorized
                .ColorizeBg(ColorService.DayColor(day))
               select background;

        private static Reader<Enviroment, string> WeekDayLine()
            => from weekDays in WeekDays()
                .OrderBy(NthDayOfWeek)
                .Select(FormattedWeekDay)
                .Sequence()
               select weekDays
                .Aggregate(new StringBuilder(), AggregateString).ToString();

        private static Reader<Enviroment, string> FormattedWeekDay(DayOfWeek day)
            => ToShortestDayName(day)
                .PadLeft(WidthOfWeekDay(day))
                .Colorize(ColorService.WeekDayColor(day));

        private static IEnumerable<DayOfWeek> WeekDays()
            => Enum
                .GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>();

        private static string ToShortestDayName(DayOfWeek dayOfWeek)
            => CurrentDateTimeFormat
                .GetShortestDayName(dayOfWeek);

        private static int GetWeekOfYear(DateTime dateTime)
            => CultureInfo
                .CurrentCulture
                .Calendar
                .GetWeekOfYear(dateTime, CurrentDateTimeFormat.CalendarWeekRule, CurrentDateTimeFormat.FirstDayOfWeek);

        private static DateTimeFormatInfo CurrentDateTimeFormat
            => CultureInfo
                .CurrentCulture
                .DateTimeFormat;

        private static int WidthOfWeek()
            => WeekDays()
                .Sum(WidthOfWeekDay);

        private static string PadWeek(string formattedWeek, IGrouping<int, DateTime> week)
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

        private static bool StartsOnFirstDayOfWeek(IGrouping<int, DateTime> week)
            => NthDayOfWeek(week.First().DayOfWeek) == 0;

        private static int WidthOfWeekDay(DayOfWeek day)
            => Math
                .Max(WidthWithSeparation(day), MinDayWidth);

        private static int WidthWithSeparation(DayOfWeek day)
            => ToShortestDayName(day).Length + RowSeparationSpace;

        private static int NthDayOfWeek(DayOfWeek dayOfWeek)
            => (dayOfWeek + DaysInAWeek - CurrentDateTimeFormat.FirstDayOfWeek) % DaysInAWeek;

        private static string Spaces(int width)
            => new(SpaceCharacter, width);
    }
}