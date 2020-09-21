using System.Collections.Generic;
using System.Drawing;
using Pastel;

namespace Calendar
{
    internal static class Extensions
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

        public static string Colorize(this string input, Color color)
            => color != Color.Transparent && ColorService.Fancy
                ? input.Pastel(color)
                : input;

        public static string ColorizeBg(this string input, Color color)
            => color != Color.Transparent && ColorService.Fancy
                ? input.PastelBg(color)
                : input;
    }
}
