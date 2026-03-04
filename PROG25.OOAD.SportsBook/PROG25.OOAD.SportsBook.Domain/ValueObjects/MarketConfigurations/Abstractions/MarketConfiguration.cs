namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

public abstract record MarketConfiguration
{
    public MarketConfiguration(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Market name cannot be null or empty.", nameof(name));
        }
        Name = name.Trim();
    }
    public string Name { get; }
}