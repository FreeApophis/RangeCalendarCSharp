using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Calendar.Test
{
    internal class TestData
    {
        internal static IEnumerable<string> ReadLines(string resource)
        {
            using var stream = GetResourceStream(resource);
            using var reader = new StreamReader(stream);

            while (true)
            {
                var line = reader.ReadLine();

                if (line is null)
                {
                    yield break;
                }

                yield return line;
            }
        }

        private static Stream GetResourceStream(string resource)
            => TestAssembly().GetManifestResourceStream(ExpandResourceName(resource));

        private static Assembly TestAssembly()
            => Assembly.GetExecutingAssembly();

        private static string ExpandResourceName(string resource)
            => $"Calendar.Test.TestData.{resource}";
    }
}