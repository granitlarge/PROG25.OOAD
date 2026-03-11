namespace PROG25.OOAD.BetExchange.Domain.ValueObjects;

public record Name
{
    public static Name Anonymized { get; } = new Name("X", "X");

    public Name(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null or whitespace.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null or whitespace.", nameof(lastName));
        }

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public string FirstName { get; }
    public string LastName { get; }
}