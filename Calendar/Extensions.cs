using System.Collections.Generic;

namespace Calendar
{
    static class Extensions
    {
        public static string Center(this string text, int width)
            => new string(' ', (width - text.Length) / 2 + 1)
               + text
               + new string(' ', (width - text.Length) / 2);

        public static string JoinString(this IEnumerable<string> source, string delimiter)
            => string.Join(delimiter, source);
    }
}
