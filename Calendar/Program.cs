using System;
using Funcky.Extensions;
using System.Globalization;
using System.Linq;
using Funcky.Monads;

namespace Calendar
{

    class Program
    {
        static void Main(string[] args)
        {
            GetCultureInfo(args)
                .AndThen(cultureInfo => CultureInfo.CurrentCulture = cultureInfo);

            Console.WriteLine();
            Console.WriteLine(ConsoleCalendar.FromYear(CalendarYear(args)));
        }

        private static int CalendarYear(string[] args) =>
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
