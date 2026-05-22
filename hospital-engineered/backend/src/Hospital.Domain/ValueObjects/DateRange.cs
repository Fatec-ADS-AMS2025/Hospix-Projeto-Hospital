using Hospital.Domain.Common;

namespace Hospital.Domain.ValueObjects;

public readonly record struct DateRange
{
    public DateTime StartAt { get; }
    public DateTime EndAt { get; }

    public DateRange(DateTime startAt, DateTime endAt)
    {
        if (endAt <= startAt)
        {
            throw new DomainException("Data final deve ser posterior a data inicial.");
        }

        StartAt = startAt;
        EndAt = endAt;
    }

    public double TotalHours => (EndAt - StartAt).TotalHours;
}
