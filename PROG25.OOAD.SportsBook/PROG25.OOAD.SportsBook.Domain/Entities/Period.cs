using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Entities;

/// <summary>
/// The Period class represents a specific time segment within an event, such as "First Half", "Second Half", "Overtime", etc.
/// What types of periods are there?
/// - In soccer, you typically have "First Half", "Second Half", "Overtime 1", "Overtime 2", "Penalty shootout": the number of periods 
/// </summary>
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