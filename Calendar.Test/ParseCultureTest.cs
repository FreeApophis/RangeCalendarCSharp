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
            { Array.Empty<string>(), Option<CultureInfo>.None },
            { new[] { "de-de" }, Option.Some(new CultureInfo("de-DE")) },
            { new[] { "2020", "de-de" }, Option.Some(new CultureInfo("de-DE")) },
            { new[] { "2000", "2005", "de-de", "fancy" }, Option.Some(new CultureInfo("de-DE")) },
            { new[] { "en-US" }, Option.Some(new CultureInfo("en-US")) },
            { new[] { "en-US", "de-de" }, Option.Some(new CultureInfo("en-US")) },
            { new[] { "de" }, Option.Some(new CultureInfo("de")) },
            { new[] { "ja-JP" }, Option.Some(new CultureInfo("ja-JP")) },
            { new[] { "something", "different", "fancy" }, Option<CultureInfo>.None },
        };
}