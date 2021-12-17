using System.Globalization;
using Funcky.DataTypes;
using Funcky.Extensions;
using Xunit;
using static Funcky.Functional;

namespace Calendar.Test;

public class ArrangeCalendarPageTest
{
    [Fact]
    public void ArrangeCalendarPageWorksAsExpected()
    {
        CultureHelper.SetAllCultures(new CultureInfo("de-CH"));

        var arrangePage = ConsoleCalendar.ArrangeCalendarPage(new CalendarFormat.SingleYear(2000));
        var result = arrangePage(new Environment(false, "MMMM yyyy")).ToList();

        CheckEquality(TestData.ReadLines("Calendar2000deCH"), result);
    }

    private static void CheckEquality(IEnumerable<string> expected, IEnumerable<string> actual)
        => expected
            .ZipLongest(actual)
            .ForEach(CheckLine);

    private static void CheckLine(EitherOrBoth<string, string> line)
        => Assert.True(line.Match(False, False, CompareLines), Message(line));

    private static bool CompareLines(string expected, string actual)
    {
        Assert.Equal(expected.TrimEnd(), actual.TrimEnd());

        return true;
    }

    private static string Message(EitherOrBoth<string, string> line)
        => line.Match(
            left => $"Right missing! left = {left}",
            right => $"Left missing! right = {right}",
            (_, _) => string.Empty);
}
