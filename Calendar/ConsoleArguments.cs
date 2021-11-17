using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Funcky;
using Funcky.Extensions;
using Funcky.Monads;
using Nager.Date;
using static Funcky.Functional;

namespace Calendar;

internal static class ConsoleArguments
{
    public static Option<CultureInfo> GetCultureInfo(IEnumerable<string> args)
        => SelectArgument(args, ToCultureInfo);

    public static CountryCode CountryFromCulture()
        => TwoLetterIsoRegionNameFromCulture()
            .ParseEnumOrNone<CountryCode>()
            .GetOrElse(() => throw new Exception("Unknown country code"));

    public static Option<int> EndYear(string[] args)
        => Should(GetStreamingMode(args))
            ? Option<int>.None()
            : GetCalendarYear(args);

    public static int GetCalendarYear(IEnumerable<string> args)
        => SelectArgument(args, ParseExtensions.ParseIntOrNone)
            .GetOrElse(DateTime.Now.Year);

    public static Environment GetEnvironment(string[] args)
        => new(GetFancyMode(args).Match(false, True));

    private static Option<CultureInfo> ToCultureInfo(string cultureString)
        => CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .Where(culture => culture.Name == cultureString)
            .FirstOrNone();

    private static Option<Unit> GetStreamingMode(IEnumerable<string> args)
        => SelectArgument(args, HasArgument("stream"));

    private static Option<Unit> GetFancyMode(IEnumerable<string> args)
        => SelectArgument(args, HasArgument("fancy"));

    private static Option<T> SelectArgument<T>(IEnumerable<string> args, Func<string, Option<T>> selector)
        where T : notnull
        => args
            .WhereSelect(selector)
            .FirstOrNone();

    private static Func<string, Option<Unit>> HasArgument(string givenArgument)
        => argument
            => argument == givenArgument
                ? Option.Some(Unit.Value)
                : Option<Unit>.None();

    private static string TwoLetterIsoRegionNameFromCulture()
        => new RegionInfo(CultureInfo.CurrentCulture.LCID).TwoLetterISORegionName;

    private static bool Should(Option<Unit> unitOption)
        => unitOption
            .Match(false, True);
}
