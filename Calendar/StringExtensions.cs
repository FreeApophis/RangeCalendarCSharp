using System.Drawing;
using Funcky.Monads;
using Pastel;

namespace Calendar;

internal static class StringExtensions
{
    public static string Center(this string text, int width)
        => (width - text.Length) switch
        {
            0 => text,
            1 => $" {text}",
            _ => Center($" {text} ", width),
        };

    public static string ApplyCalendarFormat(this CalendarFormat calendarFormat)
        => calendarFormat.Match(
            singleYear: FormatSingleYear,
            fromYear: FormatFromYear,
            yearRange: FormatYearRange);

    public static Reader<Environment, string> Colorize(this string input, Color color)
        => from shouldColorize in ShouldColorize(color)
           select shouldColorize
                ? input.Pastel(color)
                : input;

    public static Reader<Environment, string> ColorizeBg(this string input, Color color)
        => from shouldColorize in ShouldColorize(color)
           select shouldColorize
                ? input.PastelBg(color)
                : input;

    private static Reader<Environment, bool> ShouldColorize(Color color)
        => environment
            => color != Color.Transparent && environment.IsFancy;

    private static string FormatYearRange(CalendarFormat.YearRange yearRange)
    => string.Format(Resource.YearRangeFormat, yearRange.StartYear, yearRange.EndYear);

    private static string FormatFromYear(CalendarFormat.FromYear fromYear)
        => string.Format(Resource.FromYearFormat, fromYear.StartYear);

    private static string FormatSingleYear(CalendarFormat.SingleYear singleYear)
        => string.Format(Resource.SingleYearFormat, singleYear.Year);
}
