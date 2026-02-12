using Calendar;
using Funcky.Extensions;
using static System.Console;
using static Calendar.ConsoleArguments;

args
    .GetCultureInfo()
    .AndThen(CultureHelper.SetAllCultures);

var arrangePage = args
    .GetCalendarFormat()
    .ArrangeCalendarPage();

arrangePage(GetEnvironment(args))
    .ForEach(WriteLine);

await foreach (var val in Test(args).Intersperse("---"))
{
    WriteLine(val);
}

async IAsyncEnumerable<string> Test(string[] args)
{
    foreach (var arg in args)
    {
        yield return arg;
    }
}
