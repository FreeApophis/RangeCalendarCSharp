#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
using System.Globalization;
using Funcky.Monads;
using Xunit;

namespace Calendar.Test;

public class ParseCultureTest
{
    [Theory]
    [MemberData(nameof(GetDifferentProgramInputs))]
    public void TheArgumentsPassedInReturnAValidCulture(string[] arguments, Option<CultureInfo> calendarFormat)
        => Assert.Equal(calendarFormat, arguments.GetCultureInfo());

    public static TheoryData<string[], Option<CultureInfo>> GetDifferentProgramInputs()
        => new()
        {
            { [], Option<CultureInfo>.None },
            { ["de-de"], Option.Some(new CultureInfo("de-DE")) },
            { ["2020", "de-de"], Option.Some(new CultureInfo("de-DE")) },
            { ["2000", "2005", "de-de", "fancy"], Option.Some(new CultureInfo("de-DE")) },
            { ["en-US"], Option.Some(new CultureInfo("en-US")) },
            { ["en-US", "de-de"], Option.Some(new CultureInfo("en-US")) },
            { ["de"], Option.Some(new CultureInfo("de")) },
            { ["ja-JP"], Option.Some(new CultureInfo("ja-JP")) },
            { ["something", "different", "fancy"], Option<CultureInfo>.None },
        };
}