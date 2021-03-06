using Funcky.DataTypes;
using Funcky.Extensions;
using System.Collections.Generic;
using System.Globalization;
using Xunit;
using static Funcky.Functional;

namespace Calendar.Test
{
    public class ArrangeCalendarPageTest
    {
        [Fact]
        public void ArrangeCalendarPageWorksAsExpected()
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-CH");

            var arrangePage = ConsoleCalendar.ArrangeCalendarPage(2000, 2000);

            CheckEquality(TestData.ReadLines("Calendar2000deCH"), arrangePage(new Environment(false)));
        }

        private static void CheckEquality(IEnumerable<string> expected, IEnumerable<string> actual)
            => expected
                .ZipLongest(actual)
                .ForEach(CheckLine);

        private static void CheckLine(EitherOrBoth<string, string> line)
            => Assert.True(line.Match(False, False, (reference, actual) => reference.TrimEnd() == actual.TrimEnd()), Message(line));

        private static string Message(Funcky.DataTypes.EitherOrBoth<string, string> line)
            => line.Match(left => $"Right missing! left = {left}", right => $"Left missing! right = {right}", (left, right) => $"{left} and {right}");
    }
}
