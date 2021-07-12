using System;
using System.Globalization;
using Funcky.Extensions;
using static Calendar.ConsoleArguments;

namespace Calendar
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            _ = GetCultureInfo(args)
                .AndThen(cultureInfo => CultureInfo.CurrentCulture = cultureInfo);

            var arrangePage = ConsoleCalendar
                .ArrangeCalendarPage(GetCalendarYear(args), EndYear(args));

            arrangePage(GetEnvironment(args))
                .ForEach(Console.WriteLine);
        }
    }
}
