using System.Globalization;
using Funcky.Extensions;
using static System.Console;
using static Calendar.ConsoleArguments;

namespace Calendar;

internal class Program
{
    private static void Main(string[] args)
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
