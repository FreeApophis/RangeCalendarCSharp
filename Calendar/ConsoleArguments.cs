using System;
using System.Globalization;
using Funcky;
using Funcky.Extensions;
using Funcky.Monads;
using System.Linq;
using Nager.Date;
using static Funcky.Functional;

namespace Calendar
{
    internal static class ConsoleArguments
    {
        public static int GetCalendarYear(string[] args)
            => SelectArgument(args, ParseExtensions.TryParseInt)
                .GetOrElse(DateTime.Now.Year);

        public static Option<CultureInfo> GetCultureInfo(string[] args)
            => SelectArgument(args, ToCultureInfo);

        public static Option<Unit> GetStreamingMode(string[] args)
            => SelectArgument(args, Curry<string, string, Option<Unit>>(HasArgument)("stream"));

        public static Option<Unit> GetFancyMode(string[] args)
            => SelectArgument(args, Curry<string, string, Option<Unit>>(HasArgument)("fancy"));


        private static Option<T> SelectArgument<T>(string[] args, Func<string, Option<T>> selector)
            where T : notnull
            => args
                .WhereSelect(selector)
                .FirstOrNone();

        private static Option<CultureInfo> ToCultureInfo(string cultureString)
            => CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(culture => culture.Name == cultureString)
                .FirstOrNone();

        private static Option<Unit> HasArgument(string givenArgument, string argument)
            => argument == givenArgument
                ? Option.Some(Unit.Value)
                : Option<Unit>.None();

        public static CountryCode CountryFromCulture()
            => TwoLetterIsoRegionNameFromCulture()
                .TryParseEnum<CountryCode>()
                .GetOrElse(() => throw new Exception("ooops unknown country code???"));

        private static string TwoLetterIsoRegionNameFromCulture()
            => new RegionInfo(CultureInfo.CurrentCulture.LCID).TwoLetterISORegionName;
    }
}
