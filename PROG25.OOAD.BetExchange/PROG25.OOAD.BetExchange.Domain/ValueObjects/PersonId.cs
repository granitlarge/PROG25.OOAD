namespace PROG25.OOAD.BetExchange.Domain.ValueObjects;

public abstract record PersonId(PersonIdType Type, string Value)
{

}

public sealed record AnonymizedPersonId : PersonId
{
    public static AnonymizedPersonId Instance { get; } = new AnonymizedPersonId();

    private AnonymizedPersonId() : base(PersonIdType.Anonymized, "ANONYMIZED")
    {

    }
}

public record SwedishPersonalNumber : PersonId
{
    public SwedishPersonalNumber(string value) : base(PersonIdType.SwedishPersonalNumber, value)
    {
        if (!IsValidSwedishPersonalNumber(value))
            throw new ArgumentException("Invalid Swedish personal number format.", nameof(value));
        Value = value;
    }

    private static bool IsValidSwedishPersonalNumber(string personalNumber)
    {
        if (string.IsNullOrWhiteSpace(personalNumber))
            return false;
        // Implement validation logic for Swedish personal numbers here
        // This is a placeholder implementation and should be replaced with actual validation
        return personalNumber.Length == 10 && long.TryParse(personalNumber, out _);
    }
}

public enum PersonIdType
{
    Anonymized,
    SwedishPersonalNumber
}