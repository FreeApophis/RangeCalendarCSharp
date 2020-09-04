using System;
using System.Collections.Generic;
using System.Linq;
using Funcky.Extensions;
using System.Globalization;
using System.Text;
using Funcky;
using MoreLinq;
using System.Runtime.CompilerServices;

namespace Calendar
{
    static class Extensions
    {
        public static string Center(this string text, int width)
            => new string(' ', (width - text.Length) / 2 + 1)
               + text
               + new string(' ', (width - text.Length) / 2);

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            if (chunkSize <= 0) throw new ArgumentException("Chunk size must be greater than zero.", "chunkSize");

            while (source.Any())
            {
                yield return source.Take(chunkSize);
                source = source.Skip(chunkSize);
            }
        }

        public static string JoinString(this IEnumerable<string> source, string delimiter)
            => string.Join(delimiter, source);
    }

    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Console.WriteLine($"{CultureInfo.CurrentCulture.NativeName}");
            Console.WriteLine($"{CalendarYear(args)}");
            Console.WriteLine(CreateCalendarString(CalendarYear(args)));
        }

        private static int CalendarYear(string[] args) =>
            args
                .WhereSelect(cli => cli.TryParseInt())
                .FirstOrNone()
                .GetOrElse(DateTime.Now.Year);

        private static string CreateCalendarString(int year) =>
            DaysInYear(year)
                .GroupAdjacent(d => d.Month)
                .Select(LayoutMonth)
                .Chunk(3)
                .Select(m => m.Transpose())
                .Select(JoinLine)
                .SelectMany(Functional.Identity)
                .JoinString(Environment.NewLine);

        private static IEnumerable<string> JoinLine(IEnumerable<IEnumerable<string>> transposed)
            => transposed.Select(t => string.Join(' ', t));

        private static IEnumerable<string> LayoutMonth(IEnumerable<DateTime> month)
        {
            yield return MonthName(month);
            yield return WeekDayLine();

            foreach (var week in month.GroupBy(d => GetWeekOfYear(d)))
            {
                var stringBuilder = new StringBuilder();
                week.FirstOrNone().AndThen(d => stringBuilder.Append(new string(' ', 3 * NthDayOfWeek(d.DayOfWeek))));

                var result = week.Aggregate(stringBuilder,
                    (r, day) => r.AppendFormat("{0,3}", day.Day));

                yield return $"{result,21}";
            }

            yield return $"{string.Empty,21}";
        }

        private static string WeekDayLine()
            => WeekDays()
                .OrderBy(NthDayOfWeek)
                .Select(ToShortestDayName)
                .Aggregate(string.Empty, (s, d) => s = s + $"{d,3}");

        private static IEnumerable<DayOfWeek> WeekDays()
            => Enum
                .GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>();

        private static int LengthOfWeek()
            => WeekDays()
                .Count();

        private static int NthDayOfWeek(DayOfWeek dayOfWeek)
            => (dayOfWeek + LengthOfWeek() - CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) % LengthOfWeek();


        private static string ToShortestDayName(DayOfWeek dayOfWeek)
            => CultureInfo.CurrentCulture.DateTimeFormat.GetShortestDayName(dayOfWeek);

        private static object GetWeekOfYear(in DateTime dateTime)
        {
            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo cultureInfo = new CultureInfo("en-US");

            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
        }

        private static string MonthName(IEnumerable<DateTime> month) =>
            month
                .FirstOrNone()
                .Match("No Month", d => d.ToString("MMMM"))
                .Center(21);

        private static IEnumerable<DateTime> DaysInYear(int year)
        {
            var endDay = new DateTime(year + 1, 1, 1);
            var day = new DateTime(year, 1, 1);

            while (day < endDay)
            {
                yield return day;

                day = day.AddDays(1);
            }
        }
    }
}
