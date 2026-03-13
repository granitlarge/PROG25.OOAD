using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets;
using PROG25.OOAD.BetExchange.Domain.Entities;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Events;

public class Event
{
    private readonly ISet<Team> _teams;

    public Event
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

    public BooleanEventMetricMarket CreateMarket(BooleanEventMetricMarketConfiguration configuration)
    {
        return new BooleanEventMetricMarket(Id, Data, YesNoOutcome.Yes, YesNoOutcome.No, configuration);
    }
}