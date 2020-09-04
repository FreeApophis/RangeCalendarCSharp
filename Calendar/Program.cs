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
                .Batch(3)
                .Select(m => m.Transpose())
                .Select(JoinLine)
                .SelectMany(Functional.Identity)
                .JoinString(Environment.NewLine);

        private static IEnumerable<string> JoinLine(IEnumerable<IEnumerable<string>> transposed)
            => transposed.Select(t => string.Join(' ', t));

        private static IEnumerable<string> LayoutMonth(IEnumerable<DateTime> month)
            => MonthName(month).ToEnumerable()
                .Concat(WeekDayLine().ToEnumerable())
                .Concat(month.GroupBy(d => GetWeekOfYear(d)).Select(FormatWeek))
                .Concat($"{string.Empty,21}".ToEnumerable());

        private static string FormatWeek(IGrouping<object, DateTime> week) 
            => $"{AggregateWeek(week),21}";

        private static StringBuilder AggregateWeek(IGrouping<object, DateTime> week) 
            => week.Aggregate(new StringBuilder(new string(' ', 3 * NthDayOfWeek(week.First().DayOfWeek))), (r, day) => r.AppendFormat("{0,3}", day.Day));

        private static string WeekDayLine()
            => WeekDays()
                .OrderBy(NthDayOfWeek)
                .Select(ToShortestDayName)
                .Aggregate(new StringBuilder(), (s, d) => s.Append($"{d,3}"))
                .ToString();

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
            => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

        private static string MonthName(IEnumerable<DateTime> month)
            => month
                .Select(d => d.ToString("MMMM"))
                .First()
                .Center(21);

        private static IEnumerable<DateTime> DaysInYear(int year)
            => MoreEnumerable
                .Generate(new DateTime(year, 1, 1), day => day + new TimeSpan(1, 0, 0, 0))
                .TakeWhile(day => day < new DateTime(year + 1, 1, 1));
    }
}
