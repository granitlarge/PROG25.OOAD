using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;
using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Events;

using EventType = ValueObjects.EventType;

public abstract class Event
{
    private readonly ISet<Team> _teams;

    protected Event
    (
        EventType matchType,
        ISet<Team> teams,
        EventData eventData
    )
    {
        _teams = teams.ToHashSet();

        Id = new EventId();
        Type = matchType;
        Data = eventData;
    }

    public EventId Id { get; private set; }
    public EventType Type { get; private set; }
    public virtual EventData Data { get; }
    public ImmutableHashSet<Team> Teams => _teams.ToImmutableHashSet();

    public ComparisonScopedEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, ChangeScopedEventMetricMarketConfiguration configuration)
    {
        return ChangeScopedEventMetricMarket.Create(Id, Data, yesOutcome, noOutcome, configuration);
    }

    public EqualScopeEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, EqualScopeEventMetricMetricMarketConfiguration configuration)
    {
        return new EqualScopeEventMetricMarket(Id, yesOutcome, noOutcome, configuration);
    }

    public ComparisonScopedEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, ExactScopedEventMetricMarketConfiguration configuration)
    {
        return ExactEventMetricMarket.Create(Id, Data, yesOutcome, noOutcome, configuration);
    }

    public OptimalEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, OptimalScopedEventMetricMarketConfiguration configuration)
    {
        return new OptimalEventMetricMarket(Id, Data, yesOutcome, noOutcome, configuration);
    }

    public OverUnderEventMetricMarket CreateMarket
    (
        OverUnderOutcome underOutcome,
        OverUnderOutcome overOutcome,
        OverUnderOutcome pushOutcome,
        OverUnderScopedEventMetricMarketConfiguration configuration
    )
    {
        return new OverUnderEventMetricMarket(Id, Data, overOutcome, underOutcome, pushOutcome, configuration);
    }
}