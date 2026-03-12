using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.Entities;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Events;

public abstract class Event
{
    private readonly ISet<Team> _teams;

    protected Event
    (
        EventType eventType,
        ISet<Team> teams,
        EventData eventData
    )
    {
        _teams = teams.ToHashSet();

        Id = new EventId();
        Type = eventType;
        Data = eventData;
    }

    public EventId Id { get; private set; }

    public EventType Type { get; private set; }

    public virtual EventData Data { get; }

    public ImmutableHashSet<Team> Teams => [.. _teams];

    public EqualityEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, EqualityEventMetricMarketConfiguration configuration)
    {
        return new EqualityEventMetricMarket(Id, Data, yesOutcome, noOutcome, configuration);
    }

    public OptimalDimensionEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, OptimalDimensionEventMetricMarketConfiguration configuration)
    {
        return new OptimalDimensionEventMetricMarket(Id, Data, yesOutcome, noOutcome, configuration);
    }

    public ComparisonEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, ComparisonEventMetricMarketConfiguration configuration)
    {
        return new ComparisonEventMetricMarket(Id, Data, yesOutcome, noOutcome, configuration);
    }

    public List<EventMetricMarket> GenerateMarkets()
    {
        var periodDefinition = Type.GetPeriod(Data);

        if (periodDefinition is null)
        {
            return [];
        }

        return [];
    }
}