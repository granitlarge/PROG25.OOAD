using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

public abstract record MarketConfiguration
{
    public MarketConfiguration(EventDataTimestamp timestamp, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Market name cannot be null or empty.", nameof(name));
        }
        Name = name.Trim();
        Timestamp = timestamp;
    }
    public string Name { get; }

    public EventDataTimestamp Timestamp {get;}
}