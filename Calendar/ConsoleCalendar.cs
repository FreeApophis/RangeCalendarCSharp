using System;
using System.Collections.Generic;
using System.Linq;
using Funcky;
using Funcky.Extensions;
using Funcky.Monads;

namespace Calendar
{
    internal class ConsoleCalendar
    {
        private const int HorizontalMonths = 3;

        public static Reader<Environment, IEnumerable<string>> ArrangeCalendarPage(int year, Option<int> endYear)
            => from layout in GetDays(year, endYear)
                .AdjacentGroupBy(ByMonth)
                .Select(MonthLayouter.DefaultLayout)
                .Sequence()
               select layout
                .Chunk<IEnumerable<string>>(HorizontalMonths)
                .Select(Transpose)
                .SelectMany(JoinLine);

        private static IEnumerable<IEnumerable<string>> Transpose(IEnumerable<IEnumerable<string>> months)
            => months
                .Transpose();

        private static IEnumerable<DateTime> GetDays(int fromYear, Option<int> toYear = default)
            => Sequence
                .Generate(new DateTime(fromYear, 1, 1), day => day + new TimeSpan(1, 0, 0, 0))
                .TakeWhile(day => toYear.Match(true, year => day < new DateTime(year + 1, 1, 1)));

        private static int ByMonth(DateTime date)
            => date.Month;

        private static IEnumerable<string> JoinLine(IEnumerable<IEnumerable<string>> lines)
            => lines.Select(JoinWithSpace);

        private static Func<IEnumerable<string>, string> JoinWithSpace
            => s
                => s.JoinToString(" ");
    }
}
