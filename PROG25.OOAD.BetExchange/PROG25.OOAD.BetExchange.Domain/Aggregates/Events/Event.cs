using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets;
using PROG25.OOAD.BetExchange.Domain.Entities;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Periods;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

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

    public List<EventMetricMarketConfiguration> GenerateMarketConfigurations()
    {
        var periodDefinition = Type.GetPeriod(Data);

        if (periodDefinition is null)
        {
            return [];
        }

        return [];
    }

    private ImmutableHashSet<EventMetricMarketConfiguration> GenerateMarketConfigurationsForPeriod(Period period)
    {
        // We need to know which metrics are valid for the given period.
        // We need to know which metric determines the winner of the period.

        // Let's start with the winner of the period. It's one of the teams/players.
        var winnerMarkets = period.WinnerRule != null ? GeneratePeriodWinnerMarketConfigurations(period.Name, period.WinnerRule, period.EndTimestamp) : [];
        List<EqualScopeEventMetricMetricMarketConfiguration> drawMarket = period.WinnerRule != null ? [GenerateDrawMarketConfiguration(period.Name, period.WinnerRule, period.EndTimestamp)] : [];

        var markets = new List<EventMetricMarketConfiguration>();
        markets.AddRange(winnerMarkets);
        markets.AddRange(drawMarket);

        foreach (var child in period.Children)
        {
            markets.AddRange(GenerateMarketConfigurationsForPeriod(child));
        }

        return [.. markets];
    }

    private ImmutableHashSet<EventMetricMarketConfiguration> GeneratePeriodWinnerMarketConfigurations(string periodName, WinnerRule winnerRule, EventDataTimestamp periodEndTimestamp)
    {
        return [.. Teams.Select(team => new OptimalScopedEventMetricMarketConfiguration
        (
            new TeamScope(team.Id),
            winnerRule.Metric,
            periodEndTimestamp,
            $"{team.Name} to win period {periodName}",
            winnerRule.OptimumType
        ))];
    }

    private EqualScopeEventMetricMetricMarketConfiguration GenerateDrawMarketConfiguration(string periodName, WinnerRule winnerRule, EventDataTimestamp periodEndTimestamp)
    {
        var config = new EqualScopeEventMetricMetricMarketConfiguration
        (
            winnerRule.ScopeType,
            winnerRule.Metric,
            periodEndTimestamp,
            $"{periodName} will end in a draw"
        );

        return config;
    }
}