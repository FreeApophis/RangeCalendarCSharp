using System;
using System.Collections.Generic;
using System.Linq;
using Funcky;
using MoreLinq;

namespace Calendar
{
    internal class ConsoleCalendar
    {
        private const int HorizontalMonths = 3;

        internal static string FromYear(int year)
            => DaysInYear(year)
                .GroupAdjacent(ByMonth)
                .Select(MonthLayouter.DefaultLayout)
                .Batch(HorizontalMonths)
                .Select(m => m.Transpose())
                .Select(JoinLine)
                .SelectMany(Functional.Identity)
                .JoinString(Environment.NewLine);

        private static IEnumerable<DateTime> DaysInYear(int year)
            => MoreEnumerable
                .Generate(new DateTime(year, 1, 1), day => day + new TimeSpan(1, 0, 0, 0))
                .TakeWhile(day => day < new DateTime(year + 1, 1, 1));

        private static int ByMonth(DateTime date)
            => date.Month;

        private static IEnumerable<string> JoinLine(IEnumerable<IEnumerable<string>> lines)
            => lines.Select(line => string.Join(' ', line));
    }
}
