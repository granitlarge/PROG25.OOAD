namespace PROG25.OOAD.BetExchange.Domain.ValueObjects;

public record EmailAddress
{
    public static readonly EmailAddress Anonymized = new("anon@anon.com");

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email address cannot be empty.", nameof(value));
        if (!value.Contains('@'))
            throw new ArgumentException("Email address must contain '@'.", nameof(value));
        Value = value;
    }
    public string Value { get; }
}