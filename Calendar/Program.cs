using System;
using System.Collections.Generic;
using System.Linq;
using Funcky.Extensions;
using System.Globalization;
using System.Text;
using Funcky;
using MoreLinq;

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

        public static string JoinString(this IEnumerable<string> source)
            => string.Join(Environment.NewLine, source);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cultureInfo = CultureInfo.CurrentCulture;

            Console.WriteLine(cultureInfo.Name);
            Console.WriteLine(cultureInfo.NativeName);
            Console.WriteLine(cultureInfo.DateTimeFormat.CalendarWeekRule);
            Console.WriteLine(cultureInfo.DateTimeFormat.FirstDayOfWeek);

            Console.WriteLine(CreateCalendarString(CalendarYear(args)));
        }

        private static int CalendarYear(string[] args) =>
            args
                .FirstOrNone()
                .AndThen(cli => cli.TryParseInt())
                .GetOrElse(DateTime.Now.Year);

        private static string CreateCalendarString(int year) =>
            DaysInYear(year)
                .GroupAdjacent(d => d.Month)
                .Select(LayoutMonth)
                .Chunk(3)
                .Select(Transpose)
                .Select(JoinLine)
                .SelectMany(Functional.Identity)
                .JoinString();

        public static IEnumerable<IEnumerable<T>> Transpose<T>(IEnumerable<IEnumerable<T>> source)
        {
            var enumerators = source.Select(e => e.GetEnumerator()).ToArray();
            try
            {
                while (enumerators.All(e => e.MoveNext()))
                {
                    yield return enumerators.Select(e => e.Current).ToArray();
                }
            }
            finally
            {
                Array.ForEach(enumerators, e => e.Dispose());
            }
        }


        private static IEnumerable<string> JoinLine(IEnumerable<IEnumerable<string>> transposed)
            => transposed.Select(t => string.Join(' ', t));

        private static IEnumerable<string> LayoutMonth(IEnumerable<DateTime> month)
        {
            yield return MonthName(month);
            yield return Week();

            foreach (var week in month.GroupBy(d => GetWeekOfYear(d)))
            {
                var stringBuilder = new StringBuilder();
                week.FirstOrNone().AndThen(d => stringBuilder.Append(new string(' ', 3 * (int)d.DayOfWeek)));

                var result = week.Aggregate(stringBuilder,
                    (r, day) => r.AppendFormat("{0,3}", day.Day));

                yield return $"{result,21}";
            }

            yield return $"{string.Empty,21}";
        }

        // use ShortestDayNames
        // https://docs.microsoft.com/en-us/dotnet/api/system.globalization.datetimeformatinfo.shortestdaynames?view=netcore-3.1

        private static string Week()
            => " Su Mo Tu We Th Fr Sa";

        private static object GetWeekOfYear(in DateTime dateTime)
        {
            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo cultureInfo = new CultureInfo("en-US");
            System.Globalization.Calendar calendar = cultureInfo.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
            DayOfWeek dayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            return calendar.GetWeekOfYear(dateTime, calendarWeekRule, dayOfWeek);
        }

        private static string MonthName(IEnumerable<DateTime> month) =>
            month
                .FirstOrNone()
                .Match("No Month", d => d.ToString("MMMM"))
                .Center(21);

        private static IEnumerable<DateTime> DaysInYear(int year)
        {
            var endDay = new DateTime(year + 1 1, 1);
            var day = new DateTime(year, 1, 1);

            while (day < endDay)
            {
                yield return day;

                day = day.AddDays(1);
            }
        }
    }
}
