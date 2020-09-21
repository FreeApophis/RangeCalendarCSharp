using System;
using System.Globalization;
using Funcky;
using Funcky.Extensions;
using static Calendar.ConsoleArguments;

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

            GetStreamingMode(args)
                .Match(PrintSingleYear(args), PrintStream(args));

        }

        private static Action<Unit> PrintStream(string[] args) 
            => _ => ConsoleCalendar.Stream(GetCalendarYear(args)).ForEach(Console.WriteLine);

        private static Action PrintSingleYear(string[] args)
            => () => Console.WriteLine(ConsoleCalendar.SingleYear(GetCalendarYear(args)));
    }
}
