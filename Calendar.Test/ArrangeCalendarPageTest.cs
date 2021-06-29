using Funcky.Extensions;
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

            var arrangePage = ConsoleCalendar
                .ArrangeCalendarPage(2000, 2000);

            foreach (var line in TestData.ReadLines("Calendar2000deCH").ZipLongest(arrangePage(new Enviroment(false))))
            {
                Assert.True(line.Match(False, False, (reference, actual) => reference.TrimEnd() == actual.TrimEnd()), Message(line));
            }
        }

        private string Message(Funcky.DataTypes.EitherOrBoth<string, string> line)
            => line.Match(left => "Right missing!", right => "Left missing!", (left, right) => $"{left} and {right}");
    }
}
