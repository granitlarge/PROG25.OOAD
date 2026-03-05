using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;
using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Events;

public abstract class Event
{
    private readonly ISet<Team> _teams;

    protected Event
    (
        EventTypeEnum eventType,
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
    public EventTypeEnum Type { get; private set; }
    public virtual EventData Data { get; }
    public ImmutableHashSet<Team> Teams => [.. _teams];

    public EqualScopeEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, EqualScopeEventMetricMetricMarketConfiguration configuration)
    {
        return new EqualScopeEventMetricMarket(Id, Data, yesOutcome, noOutcome, configuration);
    }

    public OptimalScopedEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, OptimalScopedEventMetricMarketConfiguration configuration)
    {
        return new OptimalScopedEventMetricMarket(Id, Data, yesOutcome, noOutcome, configuration);
    }
    public ComparisonScopedEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, ComparisonScopedEventMetricMarketConfiguration configuration)
    {
        return new ComparisonScopedEventMetricMarket(Id, Data, yesOutcome, noOutcome, configuration);
    }
}