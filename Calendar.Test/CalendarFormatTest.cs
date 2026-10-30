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
            { [], new SingleYearFormat(DateTime.Now.Year) },
            { ["2020"], new SingleYearFormat(2020) },
            { ["2020", "2020"], new SingleYearFormat(2020) },
            { ["fancy"], new SingleYearFormat(DateTime.Now.Year) },
            { ["stream"], new FromYearFormat(DateTime.Now.Year) },
            { ["2005", "2001"], new SingleYearFormat(2005) },
            { ["2000", "2005"], new YearRangeFormat(2000, 2005) },
            { ["2000", "stream"], new FromYearFormat(2000) },
            { ["2005", "2020", "stream"], new FromYearFormat(2005) },
            { ["2020", "2020", "stream"], new FromYearFormat(2020) },
            { ["something", "different", "fancy"], new SingleYearFormat(DateTime.Now.Year) },
        };
}