using Xunit;

namespace Calendar.Test
{
    public class CalendarFormatTest
    {
        [Theory]
        [MemberData(nameof(GetDifferentProgramInputs))]
        public void GetCalendarFormatReturnsTheArgumentsParsedToADiscriminatedUnionOfCorrectType(string[] arguments, CalendarFormat calendarFormat)
            => Assert.Equal(calendarFormat, arguments.GetCalendarFormat());

        public static TheoryData<string[], CalendarFormat> GetDifferentProgramInputs()
            => new()
            {
                { new string[] { }, new CalendarFormat.SingleYear(DateTime.Now.Year) },
                { new[] { "2020" }, new CalendarFormat.SingleYear(2020) },
                { new[] { "2020", "2020" }, new CalendarFormat.SingleYear(2020) },
                { new[] { "fancy" }, new CalendarFormat.SingleYear(DateTime.Now.Year) },
                { new[] { "stream" }, new CalendarFormat.FromYear(DateTime.Now.Year) },
                { new[] { "2005", "2001" }, new CalendarFormat.SingleYear(2005) },
                { new[] { "2000", "2005" }, new CalendarFormat.YearRange(2000, 2005) },
                { new[] { "2000", "stream" }, new CalendarFormat.FromYear(2000) },
                { new[] { "2005", "2020", "stream" }, new CalendarFormat.FromYear(2005) },
                { new[] { "2020", "2020", "stream" }, new CalendarFormat.FromYear(2020) },
                { new[] { "something", "different", "fancy" }, new CalendarFormat.SingleYear(DateTime.Now.Year) },
            };
    }
}
