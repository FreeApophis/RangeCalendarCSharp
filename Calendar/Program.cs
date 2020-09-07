using System;
using Funcky.Extensions;
using System.Globalization;
using System.Linq;
using Funcky.Monads;
using Funcky;

namespace Calendar
{
    class Program
    {
        static void Main(string[] args)
        {
            GetCultureInfo(args)
                .AndThen(cultureInfo => CultureInfo.CurrentCulture = cultureInfo);

            GetStreamingMode(args)
                .Match(PrintSingleYear(args), PrintStream(args));
        }

        private static Action<Unit> PrintStream(string[] args) 
            => (_) => ConsoleCalendar.Stream(GetCalendarYear(args)).ForEach(Console.WriteLine);

        private static Action PrintSingleYear(string[] args)
            => () => Console.WriteLine(ConsoleCalendar.SingleYear(GetCalendarYear(args)));

        private static Option<Unit> GetStreamingMode(string[] args)
            => args
            .WhereSelect(IsStreamingArgument)
            .FirstOrNone();

        private static Option<Unit> IsStreamingArgument(string arg)
            => arg == "stream"
                ? Option.Some(Unit.Value)
                : Option<Unit>.None();

        private static int GetCalendarYear(string[] args) =>
            args
                .WhereSelect(cli => cli.TryParseInt())
                .FirstOrNone()
                .GetOrElse(DateTime.Now.Year);

        private static Option<CultureInfo> GetCultureInfo(string[] args)
            => args
            .WhereSelect(ToCultureInfo)
            .FirstOrNone();

        private static Option<CultureInfo> ToCultureInfo(string cultureString)
            => CultureInfo
                .GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures)
                .Where(culture => culture.Name == cultureString)
                .FirstOrNone();
    }
}
