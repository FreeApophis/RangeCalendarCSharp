using System.Collections.Generic;

namespace Calendar
{
    static class Extensions
    {
        public static string JoinString(this IEnumerable<string> source, string delimiter)
            => string.Join(delimiter, source);

        public static string Center(this string text, int width)
            => (width - text.Length) switch
            {
                0 => text,
                1 => $" {text}",
                _ => Center($" {text} ", width),
            };
    }
}
