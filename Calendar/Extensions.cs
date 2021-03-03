using System.Drawing;
using Pastel;

namespace Calendar
{
    internal static class Extensions
    {
        public static string Center(this string text, int width)
            => (width - text.Length) switch
            {
                0 => text,
                1 => $" {text}",
                _ => Center($" {text} ", width),
            };

        public static ColorizeString Colorize(bool fancy)
            => (input, color)
                => color != Color.Transparent && fancy
                    ? input.Pastel(color)
                    : input;

        public static ColorizeString ColorizeBg(bool fancy)
            => (input, color)
                => color != Color.Transparent && fancy
                    ? input.PastelBg(color)
                    : input;
    }
}
