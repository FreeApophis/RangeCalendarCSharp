using System.Drawing;
using Funcky.Monads;
using Pastel;

namespace Calendar;

internal static class StringExtensions
{
    extension(string text)
    {
        public string Center(int width)
            => (width - text.Length) switch
            {
                0 => text,
                1 => $" {text}",
                _ => Center($" {text} ", width),
            };

        public Reader<Environment, string> Colorize(Color color)
            => from shouldColorize in ShouldColorize(color)
               select shouldColorize
                   ? text.Pastel(color)
                   : text;

        public Reader<Environment, string> ColorizeBg(Color color)
            => from shouldColorize in ShouldColorize(color)
               select shouldColorize
                   ? text.PastelBg(color)
                   : text;
    }

    extension(CalendarFormat calendarFormat)
    {
        public string ApplyCalendarFormat()
            => calendarFormat switch
            {
                SingleYearFormat singleYear => FormatSingleYear(singleYear),
                FromYearFormat fromYear => FormatFromYear(fromYear),
                YearRangeFormat yearRange => FormatYearRange(yearRange),
            };
    }

    private static Reader<Environment, bool> ShouldColorize(Color color)
        => environment
            => environment is { IsFancy: true } && color != Color.Transparent;

    private static string FormatYearRange(YearRangeFormat yearRange)
        => string.Format(Resource.YearRangeFormat, yearRange.StartYear, yearRange.EndYear);

    private static string FormatFromYear(FromYearFormat fromYear)
        => string.Format(Resource.FromYearFormat, fromYear.StartYear);

    private static string FormatSingleYear(SingleYearFormat singleYear)
        => string.Format(Resource.SingleYearFormat, singleYear.Year);
}
