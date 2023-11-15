#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
using System.Drawing;
using Funcky;
using Funcky.Extensions;
using Funcky.Monads;

namespace Calendar;

internal static class ConsoleCalendar
{
    private const int HorizontalMonths = 3;
    private static readonly Func<IEnumerable<IEnumerable<string>>, IEnumerable<string>> JoinLine = lines => lines.Select(JoinWithSpace);
    private static readonly Func<IEnumerable<IEnumerable<string>>, IEnumerable<IEnumerable<string>>> Transpose = months => months.Transpose();

    private static int CalendarWidth
        => (HorizontalMonths * MonthLayouter.WidthOfWeek) + SeparatorsBetweenMonths();

    public static Reader<Environment, IEnumerable<string>> ArrangeCalendarPage(this CalendarFormat format)
        => from title in format.GetTitle()
           from layout in format.GetDays()
            .AdjacentGroupBy(ByMonth)
            .Select(MonthLayouter.DefaultLayout)
            .Sequence()
           let calendar = layout
            .Chunk(HorizontalMonths)
            .SelectMany(JoinLine.Compose(Transpose))
           select Sequence.Concat(title, calendar);

    private static Reader<Environment, IEnumerable<string>> GetTitle(this CalendarFormat format)
        => from title in format
            .ApplyCalendarFormat()
            .Center(CalendarWidth)
            .Colorize(Color.Yellow)
           select TitleFormat(title);

    private static IReadOnlyList<string> TitleFormat(string title)
        => [string.Empty, title, string.Empty];

    private static int SeparatorsBetweenMonths()
        => HorizontalMonths - 1;

    private static IEnumerable<DateOnly> GetDays(this CalendarFormat format)
        => format.Match(
            singleYear: DaysOfSingleYear,
            fromYear: DaysStartingWithYear,
            yearRange: DaysOfYearRange);

    private static IEnumerable<DateOnly> DaysOfYearRange(CalendarFormat.YearRange yearRange)
        => DaysFrom(yearRange.StartYear)
            .TakeWhile(day => day.Year <= yearRange.EndYear);

    private static IEnumerable<DateOnly> DaysStartingWithYear(CalendarFormat.FromYear fromYear)
        => DaysFrom(fromYear.StartYear);

    private static IEnumerable<DateOnly> DaysOfSingleYear(CalendarFormat.SingleYear singleYear)
        => DaysFrom(singleYear.Year)
            .TakeWhile(day => day.Year == singleYear.Year);

    private static IEnumerable<DateOnly> DaysFrom(int startYear)
        => Sequence.Successors(JanuaryFirst(startYear), NextDay);

    private static DateOnly NextDay(DateOnly day)
        => day.AddDays(1);

    private static DateOnly JanuaryFirst(int fromYear)
        => new(fromYear, 1, 1);

    private static int ByMonth(DateOnly date)
        => date.Month;

    private static string JoinWithSpace(IEnumerable<string> parts)
        => parts.JoinToString(' ');
}
