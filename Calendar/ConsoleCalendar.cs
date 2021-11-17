﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Funcky;
using Funcky.Extensions;
using Funcky.Monads;

namespace Calendar;

internal class ConsoleCalendar
{
    private const int HorizontalMonths = 3;

    private static int CalendarWidth
        => (HorizontalMonths * MonthLayouter.WidthOfWeek) + SeparatorsBetweenMonths();

    public static Reader<Environment, IEnumerable<string>> ArrangeCalendarPage(int year, Option<int> endYear)
        => from title in GetTitle(year, endYear)
           from layout in GetDays(year, endYear)
            .AdjacentGroupBy(ByMonth)
            .Select(MonthLayouter.DefaultLayout)
            .Sequence()
           let calendar = layout
            .Chunk(HorizontalMonths)
            .Select(Transpose)
            .SelectMany(JoinLine)
           select Sequence.Concat(title, calendar);

    private static Reader<Environment, IEnumerable<string>> GetTitle(int year, Option<int> endYear)
        => from title in Resource
            .CalendarOneYear
            .ApplyYear(year, endYear)
            .Center(CalendarWidth)
            .Colorize(Color.Yellow)
           select Sequence
            .Return(string.Empty)
            .Append(title)
            .Append(string.Empty);

    private static int SeparatorsBetweenMonths()
        => HorizontalMonths - 1;

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

    private static string JoinWithSpace(IEnumerable<string> parts)
        => parts.JoinToString(' ');
}
