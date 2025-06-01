namespace Bookify.Domain.Bookings;

public record DateRange
{
    private DateRange(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }

    public DateOnly Start { get; }
    public DateOnly End { get; }

    public int DurationInDays => End.DayNumber - Start.DayNumber;

    public static DateRange Create(DateOnly start, DateOnly end)
    {
        if (start >= end)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        return new DateRange(start, end);
    }
}
