namespace Calendar;

public closed record class CalendarFormat;

public sealed record class SingleYearFormat(int Year) : CalendarFormat;

public sealed record class FromYearFormat(int StartYear) : CalendarFormat;

public record YearRangeFormat(int StartYear, int EndYear) : CalendarFormat;
