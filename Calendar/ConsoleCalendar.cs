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
        private static readonly Func<char, IEnumerable<string>, string> Join = string.Join;

        public static Reader<Fancy, IEnumerable<string>> ArrangeCalendarPage(int year, Option<int> endYear)
            => fancy
                => GetDays(year, endYear)
                    .AdjacentGroupBy(ByMonth)
                    .Select(m => MonthLayouter.DefaultLayout(m)(fancy))
                    .Chunk(HorizontalMonths)
                    .Select(m => m.Transpose())
                    .SelectMany(JoinLine);

        private static IEnumerable<DateTime> GetDays(int fromYear, Option<int> toYear = default)
            => Sequence
                .Generate(new DateTime(fromYear, 1, 1), day => day + new TimeSpan(1, 0, 0, 0))
                .TakeWhile(day => toYear.Match(true, year => day < new DateTime(year + 1, 1, 1)));

        private static int ByMonth(DateTime date)
            => date.Month;

        private static IEnumerable<string> JoinLine(IEnumerable<IEnumerable<string>> lines)
            => lines.Select(Join.Curry()(' '));
    }
}
