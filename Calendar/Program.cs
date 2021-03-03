using System;
using System.Globalization;
using System.Linq;
using Funcky;
using Funcky.Extensions;
using Funcky.Monads;
using static Calendar.ConsoleArguments;
using static Funcky.Functional;

namespace Calendar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GetCultureInfo(args)
                .AndThen(cultureInfo => CultureInfo.CurrentCulture = cultureInfo);

            GetFancyMode(args)
                .AndThen(_ => ColorService.Fancy = true);

            ConsoleCalendar
                .ArrangeCalendarPage(GetCalendarYear(args), ShouldStream(args))
                .ForEach(Console.WriteLine);
        }

        private static Option<Unit> ShouldStream(string[] args)
            => GetStreamingMode(args);

    }
}
