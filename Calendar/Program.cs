using System;
using System.Globalization;
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

            var arrangePage = ConsoleCalendar
                .ArrangeCalendarPage(GetCalendarYear(args), EndYear(args));

            arrangePage(GetFancy(args))
                .ForEach(Console.WriteLine);
        }
    }
}
