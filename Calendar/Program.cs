using System.Globalization;
using Funcky.Extensions;
using static Calendar.ConsoleArguments;
using static System.Console;

namespace Calendar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GetCultureInfo(args)
                .AndThen(SetCulture);

            var arrangePage = ConsoleCalendar
                .ArrangeCalendarPage(GetCalendarYear(args), EndYear(args));

            arrangePage(GetEnvironment(args))
                .ForEach(WriteLine);
        }

        private static void SetCulture(CultureInfo cultureInfo)
        {
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
    }
}
