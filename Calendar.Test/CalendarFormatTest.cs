#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
using Xunit;

namespace Calendar.Test;

public class CalendarFormatTest
{
    [Theory]
    [MemberData(nameof(GetDifferentProgramInputs))]
    public void GetCalendarFormatReturnsTheArgumentsParsedToADiscriminatedUnionOfCorrectType(string[] arguments, CalendarFormat calendarFormat)
        => Assert.Equal(calendarFormat, arguments.GetCalendarFormat());

    public static TheoryData<string[], CalendarFormat> GetDifferentProgramInputs()
        => new()
        {
            { [], new CalendarFormat.SingleYear(DateTime.Now.Year) },
            { ["2020"], new CalendarFormat.SingleYear(2020) },
            { ["2020", "2020"], new CalendarFormat.SingleYear(2020) },
            { ["fancy"], new CalendarFormat.SingleYear(DateTime.Now.Year) },
            { ["stream"], new CalendarFormat.FromYear(DateTime.Now.Year) },
            { ["2005", "2001"], new CalendarFormat.SingleYear(2005) },
            { ["2000", "2005"], new CalendarFormat.YearRange(2000, 2005) },
            { ["2000", "stream"], new CalendarFormat.FromYear(2000) },
            { ["2005", "2020", "stream"], new CalendarFormat.FromYear(2005) },
            { ["2020", "2020", "stream"], new CalendarFormat.FromYear(2020) },
            { ["something", "different", "fancy"], new CalendarFormat.SingleYear(DateTime.Now.Year) },
        };
}