using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Entities;

public class Period
{
    public Period(string name)
    {
        if (!IsValidPeriodName(name))
        {
            throw new ArgumentException("Invalid period name.");
        }

        Id = new PeriodId();
        Name = name;
    }

    public PeriodId Id { get; }
    public string Name { get; }

    private static bool IsValidPeriodName(string name)
    {
        // Implement validation logic for period names if needed
        return !string.IsNullOrWhiteSpace(name);
    }
}