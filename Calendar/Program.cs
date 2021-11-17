using Calendar;
using Funcky.Extensions;
using static System.Console;
using static Calendar.ConsoleArguments;

GetCultureInfo(args)
    .AndThen(CultureHelper.SetAllCultures);

var arrangePage = ConsoleCalendar.ArrangeCalendarPage(GetCalendarYear(args), EndYear(args));

arrangePage(GetEnvironment(args))
    .ForEach(WriteLine);