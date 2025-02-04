using System.Globalization;
using Funcky;
using Funcky.Extensions;
using Funcky.Monads;
using Nager.Date;

namespace Calendar;

internal static class ConsoleArguments
{
    private const int EndYearOffset = 1;

    public static Option<CultureInfo> GetCultureInfo(this IEnumerable<string> arguments)
        => arguments
            .SelectArgument(ToCultureInfo);

    public static Environment GetEnvironment(IEnumerable<string> arguments)
        => new(arguments.GetFancyMode() is [_], "MMMM yyyy");

    public static CountryCode CountryFromCulture()
        => TwoLetterIsoRegionNameFromCulture()
            .ParseEnumOrNone<CountryCode>()
            .GetOrElse(() => throw new Exception("Unknown country code"));

    public static CalendarFormat GetCalendarFormat(this IEnumerable<string> arguments)
        => arguments
            .GetStreamingMode()
            .Match(
                none: FixedCalendarFormat(arguments),
                some: StreamingCalendarFormat(arguments));

    private static Func<CalendarFormat> FixedCalendarFormat(IEnumerable<string> arguments)
        => ()
            => arguments
                .EndYear()
                .Match(
                    none: SingleYearFormat(arguments),
                    some: YearRangeFormat(arguments));

    private static Func<int, CalendarFormat> YearRangeFormat(IEnumerable<string> arguments)
        => endYear
            => arguments.StartYear() < endYear
                ? new CalendarFormat.YearRange(arguments.StartYear(), endYear)
                : new CalendarFormat.SingleYear(arguments.StartYear());

    private static Func<CalendarFormat> SingleYearFormat(IEnumerable<string> arguments)
        => ()
            => new CalendarFormat.SingleYear(arguments.StartYear());

    private static Func<Unit, CalendarFormat> StreamingCalendarFormat(IEnumerable<string> arguments)
        => _
            => new CalendarFormat.FromYear(arguments.StartYear());

    private static int StartYear(this IEnumerable<string> arguments)
        => SelectArgument(arguments, ParseExtensions.ParseInt32OrNone)
            .GetOrElse(DateTime.Now.Year);

    private static Option<int> EndYear(this IEnumerable<string> arguments)
        => arguments
            .WhereSelect(ParseExtensions.ParseInt32OrNone)
            .Skip(EndYearOffset)
            .FirstOrNone();

    private static Option<CultureInfo> ToCultureInfo(string cultureString)
        => CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .Where(CultureIs(cultureString))
            .FirstOrNone();

    private static Func<CultureInfo, bool> CultureIs(string cultureString)
        => culture
            => culture.Name.Equals(cultureString, StringComparison.InvariantCultureIgnoreCase);

    private static Option<Unit> GetStreamingMode(this IEnumerable<string> arguments)
        => arguments
            .SelectArgument(HasArgument("stream"));

    private static Option<Unit> GetFancyMode(this IEnumerable<string> arguments)
        => arguments
            .SelectArgument(HasArgument("fancy"));

    private static Option<T> SelectArgument<T>(this IEnumerable<string> arguments, Func<string, Option<T>> selector)
        where T : notnull
        => arguments
            .WhereSelect(selector)
            .FirstOrNone();

    private static Func<string, Option<Unit>> HasArgument(string givenArgument)
        => argument
            => Option.FromBoolean(argument == givenArgument);

    private static string TwoLetterIsoRegionNameFromCulture()
        => CultureInfo.CurrentCulture.CultureTypes.HasFlag(CultureTypes.UserCustomCulture)
            ? FallBackTwoLetterISORegionName()
            : new RegionInfo(CultureInfo.CurrentCulture.LCID).TwoLetterISORegionName;

    private static string FallBackTwoLetterISORegionName()
        => CultureInfo.CurrentCulture.Name
            .SplitLazy('-')
            .Last();
}
