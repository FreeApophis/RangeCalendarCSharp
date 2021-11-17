using System.Reflection;
using Funcky.Extensions;

namespace Calendar.Test;

internal class TestData
{
    internal static IEnumerable<string> ReadLines(string resource)
    {
        using var stream = GetResourceStream(resource);
        using var reader = new StreamReader(stream);

        return reader.ReadLines().Materialize();
    }

    private static Stream GetResourceStream(string resource)
        => TestAssembly().GetManifestResourceStream(ExpandResourceName(resource)) ?? throw new Exception($"Resource '{resource}' could not be loaded.");

    private static Assembly TestAssembly()
        => Assembly.GetExecutingAssembly();

    private static string ExpandResourceName(string resource)
        => $"Calendar.Test.TestData.{resource}";
}
