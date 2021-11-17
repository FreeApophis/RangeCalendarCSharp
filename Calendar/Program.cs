using System.Globalization;
using Calendar;
using Funcky.Extensions;
using static System.Console;
using static Calendar.ConsoleArguments;

static void SetAllCultures(CultureInfo cultureInfo)
{
    CultureInfo.CurrentCulture = cultureInfo;
    CultureInfo.CurrentUICulture = cultureInfo;
}

GetCultureInfo(args)
    .AndThen(SetAllCultures);

var arrangePage = ConsoleCalendar.ArrangeCalendarPage(GetCalendarYear(args), EndYear(args));

arrangePage(GetEnvironment(args))
    .ForEach(WriteLine);