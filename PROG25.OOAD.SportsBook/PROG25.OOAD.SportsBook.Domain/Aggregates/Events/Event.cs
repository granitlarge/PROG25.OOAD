using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Periods;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Events;

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

    public List<EventMetricMarket> GenerateMarkets()
    {
        var periodDefinition = Type.GetPeriod(Data);

        if (periodDefinition is null)
        {
            return [];
        }

        return [];
    }

    private ImmutableHashSet<EventMetricMarket> GenerateMarketsForPeriod(Period period)
    {
        // We need to know which metrics are valid for the given period.
        // We need to know which metric determines the winner of the period.

        // Let's start with the winner of the period. It's one of the teams/players.
        var winnerMarkets = period.WinnerRule != null ? GeneratePeriodWinnerMarkets(period.Name, period.WinnerRule, period.EndTimestamp) : [];
        List<EqualScopeEventMetricMarket> drawMarket = period.WinnerRule != null ? [GenerateDrawMarket(period.Name, period.WinnerRule, period.EndTimestamp)] : [];

        var markets = new List<EventMetricMarket>();
        markets.AddRange(winnerMarkets);
        markets.AddRange(drawMarket);

        foreach (var child in period.Children)
        {
            markets.AddRange(GenerateMarketsForPeriod(child));
        }

        return [.. markets];
    }

    private ImmutableHashSet<EventMetricMarket> GeneratePeriodWinnerMarkets(string periodName, WinnerRule winnerRule, EventDataTimestamp periodEndTimestamp)
    {
        return [.. Teams.Select(team => new OptimalScopedEventMetricMarketConfiguration
        (
            new TeamScope(team.Id),
            winnerRule.Metric,
            periodEndTimestamp,
            $"{team.Name} to win period {periodName}",
            winnerRule.OptimumType
        )).Select(config => new OptimalScopedEventMetricMarket
        (
            Id,
            Data,
            new YesNoOutcome(new ValueObjects.Odds.Odds(1.5m), true),
            new YesNoOutcome(new ValueObjects.Odds.Odds(1.5m), false),
            config
        ))];
    }

    private EqualScopeEventMetricMarket GenerateDrawMarket(string periodName, WinnerRule winnerRule, EventDataTimestamp periodEndTimestamp)
    {
        var config = new EqualScopeEventMetricMetricMarketConfiguration
        (
            winnerRule.ScopeType,
            winnerRule.Metric,
            periodEndTimestamp,
            $"{periodName} will end in a draw"
        );

        return new EqualScopeEventMetricMarket
        (
            Id,
            Data,
            new YesNoOutcome(new ValueObjects.Odds.Odds(1.5m), true),
            new YesNoOutcome(new ValueObjects.Odds.Odds(1.5m), false),
            config
        );
    }
}