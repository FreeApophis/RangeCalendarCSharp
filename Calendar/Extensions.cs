using System;
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

        public static ColorizeString Colorize(ColorizePredicate shouldColorize)
            => (input, color)
                => shouldColorize(color)
                    ? input.Pastel(color)
                    : input;

        public static ColorizeString ColorizeBg(ColorizePredicate shouldColorize)
            => (input, color)
            => shouldColorize(color)
                ? input.PastelBg(color)
                : input;

        public static ColorizePredicate ShouldColorize(bool fancy)
            => color
                => color != Color.Transparent && fancy;

    }
}
