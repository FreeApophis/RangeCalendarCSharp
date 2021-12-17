using Calendar;
using Funcky.Extensions;
using static System.Console;
using static Calendar.ConsoleArguments;

GetCultureInfo(args)
    .AndThen(CultureHelper.SetAllCultures);

var arrangePage = args
    .GetCalendarFormat()
    .ArrangeCalendarPage();

arrangePage(GetEnvironment(args))
    .ForEach(WriteLine);