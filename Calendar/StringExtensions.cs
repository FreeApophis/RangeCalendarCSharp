using System.Drawing;
using Funcky.Monads;
using Pastel;

namespace Calendar
{
    internal static class StringExtensions
    {
        public static string Center(this string text, int width)
            => (width - text.Length) switch
            {
                0 => text,
                1 => $" {text}",
                _ => Center($" {text} ", width),
            };

        public static Reader<Environment, string> Colorize(this string input, Color color)
                => from shouldColorize in ShouldColorize(color)
                   select shouldColorize 
                    ? input.Pastel(color) 
                    : input;

        public static Reader<Environment, string> ColorizeBg(this string input, Color color)
                => from shouldColorize in ShouldColorize(color)
                   select shouldColorize 
                    ? input.PastelBg(color) 
                    : input;

        private static Reader<Environment, bool> ShouldColorize(Color color)
            => fancy
                => color != Color.Transparent && fancy.IsFancy;

    }
}
