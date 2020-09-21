using System;
using System.Collections.Generic;
using System.Linq;
using Funcky.Monads;
using MoreLinq;

namespace Calendar
{
    internal class ConsoleCalendar
    {
        private const int HorizontalMonths = 3;

        internal static string SingleYear(int year)
            => ArrangeCalendarPage(year, year => GetDays(year, Option.Some(year)))
                .JoinString(Environment.NewLine);

        internal static IEnumerable<string> Stream(int year)
            => ArrangeCalendarPage(year, year => GetDays(year));

        private static IEnumerable<string> ArrangeCalendarPage(int year, Func<int, IEnumerable<DateTime>> dayEnumerator)
            => dayEnumerator(year)
                .GroupAdjacent(ByMonth)
                .Select(MonthLayouter.DefaultLayout)
                .Batch(HorizontalMonths)
                .Select(m => m.Transpose())
                .SelectMany(JoinLine);

        private static IEnumerable<DateTime> GetDays(int fromYear, Option<int> toYear = default)
            => MoreEnumerable
                .Generate(new DateTime(fromYear, 1, 1), day => day + new TimeSpan(1, 0, 0, 0))
                .TakeWhile(day => toYear.Match(true, year => day < new DateTime(year + 1, 1, 1)));

        private static int ByMonth(DateTime date)
            => date.Month;

        private static IEnumerable<string> JoinLine(IEnumerable<IEnumerable<string>> lines)
            => lines.Select(line => string.Join(' ', line));
    }
}
