using System.Collections.Immutable;
using System.Drawing;
using System.Globalization;
using System.Text;
using Funcky.Extensions;
using Funcky.Monads;

namespace Calendar;

internal class MonthLayouter
{
    private const int RowSeparationSpace = 1;
    private const int MinDayWidth = 3;
    private const int DaysInAWeek = 7;
    private const char SpaceCharacter = ' ';

    public static int WidthOfWeek
        => WeekDays()
            .Sum(WidthOfWeekDay);

    private static DateTimeFormatInfo CurrentDateTimeFormat
        => CultureInfo
            .CurrentCulture
            .DateTimeFormat;

    public static Reader<Environment, IEnumerable<string>> DefaultLayout(IEnumerable<DateOnly> month)
            => from colorizedMonthName in ColorizedMonthName(month)
               from weekDayLine in WeekDayLine()
               from weeksInMonth in month
                .AdjacentGroupBy(GetWeekOfYear)
                .Select(FormatWeek)
                .Sequence()
               select BuildDefaultLayout(colorizedMonthName, weekDayLine, weeksInMonth);

    private static IEnumerable<string> BuildDefaultLayout(string colorizedMonthName, string weekDayLine, IEnumerable<string> weeks)
        => ImmutableList<string>.Empty
            .Add(colorizedMonthName)
            .Add(weekDayLine)
            .AddRange(weeks)
            .Add(Spaces(WidthOfWeek));

    private static Reader<Environment, string> ColorizedMonthName(IEnumerable<DateOnly> month)
        => from monthName in MonthName(month)
           from colorizedMonthName in monthName.Colorize(Color.White)
           select colorizedMonthName;

    private static Reader<Environment, string> MonthName(IEnumerable<DateOnly> month)
        => from monthName in FormatMonthName(month.First())
           select monthName.Center(WidthOfWeek);

    private static Reader<Environment, string> FormatMonthName(DateOnly month)
        => environment
            => month
                .ToString(environment.MonthFormat);

    private static Reader<Environment, string> FormatWeek(IGrouping<int, DateOnly> week)
        => from aggregateWeek in AggregateWeek(week)
           select PadWeek(aggregateWeek, week);

    private static Reader<Environment, string> AggregateWeek(IEnumerable<DateOnly> week)
        => from formatDay in week
            .Select(FormatDay)
            .Sequence()
           select formatDay
            .Aggregate(new StringBuilder(), AggregateString)
            .ToString();

    private static StringBuilder AggregateString(StringBuilder aggregate, string formattedString)
        => aggregate
            .Append(formattedString);

    private static Reader<Environment, string> FormatDay(DateOnly day)
        => from colorized in day
            .Day
            .ToString()
            .PadLeft(WidthOfWeekDay(day.DayOfWeek))
            .Colorize(ColorService.WeekDayColor(day.DayOfWeek))
           from background in colorized
            .ColorizeBg(ColorService.DayColor(day))
           select background;

    private static Reader<Environment, string> WeekDayLine()
        => from weekDays in WeekDays()
            .OrderBy(NthDayOfWeek)
            .Select(FormattedWeekDay)
            .Sequence()
           select weekDays
            .Aggregate(new StringBuilder(), AggregateString).ToString();

    private static Reader<Environment, string> FormattedWeekDay(DayOfWeek day)
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

    private static int GetWeekOfYear(DateOnly dateTime)
        => CultureInfo
            .CurrentCulture
            .Calendar
            .GetWeekOfYear(dateTime.ToDateTime(default), CurrentDateTimeFormat.CalendarWeekRule, CurrentDateTimeFormat.FirstDayOfWeek);

    private static string PadWeek(string formattedWeek, IGrouping<int, DateOnly> week)
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

    private static bool StartsOnFirstDayOfWeek(IEnumerable<DateOnly> week)
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
