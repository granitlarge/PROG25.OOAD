using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;
using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

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

    public ChangeEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, ValueObjects.MarketConfigurations.ComparisonScopedEventMetricMarketConfiguration configuration)
    {
        var teamPlayerPairs = _teams.SelectMany(t => t.Players.Select(p => (t.Id, p.Id))).ToHashSet();
        return new ChangeEventMetricMarket(Id, Data, teamPlayerPairs, yesOutcome, noOutcome, configuration);
    }

    public EqualScopedEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, EqualScopedEventMetricMarketConfiguration configuration)
    {
        return new EqualScopedEventMetricMarket(Id, Data, yesOutcome, noOutcome, configuration);
    }

    public ComparisonScopedEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, ExactEventMetricMarketConfiguration configuration)
    {
        var teamPlayerPairs = _teams.SelectMany(t => t.Players.Select(p => (t.Id, p.Id))).ToHashSet();
        return ExactEventMetricMarket.Create(Id, Data, teamPlayerPairs, yesOutcome, noOutcome, configuration);
    }

    public OptimalEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, OptimalEventMetricMarketConfiguration configuration)
    {
        var teamPlayerPairs = _teams.SelectMany(t => t.Players.Select(p => (t.Id, p.Id))).ToHashSet();
        return new OptimalEventMetricMarket(Id, Data, teamPlayerPairs, yesOutcome, noOutcome, configuration);
    }

    public OverUnderEventMetricMarket CreateMarket
    (
        OverUnderOutcome underOutcome,
        OverUnderOutcome overOutcome,
        OverUnderOutcome pushOutcome,
        OverUnderEventMetricMarketConfiguration configuration
    )
    {
        var teamPlayerPairs = _teams.SelectMany(t => t.Players.Select(p => (t.Id, p.Id))).ToHashSet();
        return new OverUnderEventMetricMarket(Id, Data, teamPlayerPairs, overOutcome, underOutcome, pushOutcome, configuration);
    }
}