using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Funcky;
using Funcky.Extensions;
using Funcky.Monads;
using static Calendar.ConsoleArguments;
using static Funcky.Functional;

namespace Calendar
{
    public delegate string ColorizeString(string input, Color color);
    public delegate bool ColorizePredicate(Color color);

    internal class Program
    {
        static void Main(string[] args)
        {
            GetCultureInfo(args)
                .AndThen(cultureInfo => CultureInfo.CurrentCulture = cultureInfo);

            ArrangeCalendarPage(args, ShouldColorize(args))
                .ForEach(Console.WriteLine);
        }

        private static IEnumerable<string> ArrangeCalendarPage(string[] args, ColorizePredicate shouldColorize)
            => CompositionRoot(
                GetCalendarYear(args),
                EndYear(args),
                Extensions.Colorize(shouldColorize),
                Extensions.ColorizeBg(shouldColorize));

        private static ColorizePredicate ShouldColorize(IEnumerable<string> args)
            => Extensions.ShouldColorize(Should(GetFancyMode(args)));

        private static IEnumerable<string> CompositionRoot(int year, Option<int> endYear, ColorizeString colorizeString, ColorizeString colorizeStringBackground)
            => ConsoleCalendar
                .ArrangeCalendarPage(
                    MonthLayout.Default(
                        MonthLayout.MonthName(colorizeString),
                        MonthLayout.WeekDayLine(
                            MonthLayout.AggregateWeekDays(
                                MonthLayout.FormattedWeekDay(colorizeString))),
                        MonthLayout.WeeksOfMonth(
                            MonthLayout.FormatWeek(
                                MonthLayout.AggregateWeek(
                                    MonthLayout.AggregateDays(
                                        MonthLayout.FormatDay(
                                            colorizeString,
                                            colorizeStringBackground)))))),
                    year,
                    endYear);

        private static Option<int> EndYear(string[] args)
            => Should(GetStreamingMode(args))
                ? Option<int>.None()
                : GetCalendarYear(args);

        private static bool Should(Option<Unit> unitOption)
        => unitOption
            .Match(false, True);
    }
}
