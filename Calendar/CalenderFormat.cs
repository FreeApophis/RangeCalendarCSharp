using Funcky;

namespace Calendar;

[DiscriminatedUnion]
public abstract partial record CalendarFormat
{
    public partial record SingleYear(int Year) : CalendarFormat;

    public partial record FromYear(int StartYear) : CalendarFormat;

    public partial record YearRange(int StartYear, int EndYear) : CalendarFormat;
}