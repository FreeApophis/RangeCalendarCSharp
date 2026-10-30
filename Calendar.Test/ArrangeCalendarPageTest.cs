using System.Globalization;
using Funcky;
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

        var arrangePage = new SingleYearFormat(2000).ArrangeCalendarPage();

        var result = arrangePage(new Environment(false, "MMMM yyyy")).Materialize();

        CheckEquality(TestData.ReadLines("Calendar2000deCH"), result);
    }

    private static void CheckEquality(IEnumerable<string> expected, IEnumerable<string> actual)
        => expected
            .ZipLongest(actual)
            .ForEach(CheckLine);

    private static void CheckLine(EitherOrBoth<string, string> line)
        => Assert.True(Match(line), Message(line));

    private static bool Match(EitherOrBoth<string, string> line)
        => line.Match(
            left: False,
            right: False,
            both: CompareLines);

    private static bool CompareLines(string expected, string actual)
    {
        Assert.Equal(expected.TrimEnd(), actual.TrimEnd());

        return true;
    }

    private static string Message(EitherOrBoth<string, string> line)
        => line.Match(
            left: left => $"Right missing! left = {left}",
            right: right => $"Left missing! right = {right}",
            both: (_, _) => string.Empty);
}
