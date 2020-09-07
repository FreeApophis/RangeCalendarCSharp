using System;
using System.Globalization;
using Funcky;
using Funcky.Extensions;
using Funcky.Monads;
using System.Linq;

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
            => SelectArgument(args, IsStreamingArgument);

        private static Option<T> SelectArgument<T>(string[] args, Func<string, Option<T>> selector) 
            => args
                .WhereSelect(selector)
                .FirstOrNone();

        private static Option<CultureInfo> ToCultureInfo(string cultureString)
            => CultureInfo
                .GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures)
                .Where(culture => culture.Name == cultureString)
                .FirstOrNone();

        private static Option<Unit> IsStreamingArgument(string arg)
            => arg == "stream"
                ? Option.Some(Unit.Value)
                : Option<Unit>.None();
    }
}
