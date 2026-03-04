using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;
using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Events;

using EventType = ValueObjects.EventType;

public abstract class Event
{
    private readonly HashSet<Team> _teams;

    protected Event
    (
        EventType matchType,
        HashSet<Team> teams,
        EventStatistics eventStats,
        DateTimeOffset startDate
    )
    {
        _teams = teams.ToHashSet();

        Id = new EventId();
        Type = matchType;
        Statistics = eventStats;
        StartDate = startDate;
    }

    public EventId Id { get; private set; }
    public EventType Type { get; private set; }
    public virtual EventStatistics Statistics { get; }
    public DateTimeOffset StartDate { get; private set; }
    public ImmutableHashSet<Team> Teams => _teams.ToImmutableHashSet();

    public ChangeEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, ChangeEventMetricMarketConfiguration configuration)
    {
        return new ChangeEventMetricMarket(this, yesOutcome, noOutcome, configuration);
    }

    public EqualScopedEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, EqualScopedEventMetricMarketConfiguration configuration)
    {
        return new EqualScopedEventMetricMarket(this, yesOutcome, noOutcome, configuration);
    }

    public ExactEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, ExactEventMetricMarketConfiguration configuration)
    {
        return new ExactEventMetricMarket(this, yesOutcome, noOutcome, configuration);
    }

    public OptimalEventMetricMarket CreateMarket(YesNoOutcome yesOutcome, YesNoOutcome noOutcome, OptimalEventMetricMarketConfiguration configuration)
    {
        return new OptimalEventMetricMarket(this, yesOutcome, noOutcome, configuration);
    }

    public OverUnderEventMetricMarket CreateMarket
    (
        OverUnderOutcome underOutcome,
        OverUnderOutcome overOutcome,
        OverUnderOutcome pushOutcome,
        OverUnderEventMetricMarketConfiguration configuration
    )
    {
        return new OverUnderEventMetricMarket(this, overOutcome, underOutcome, pushOutcome, configuration);
    }
}