using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Periods;

public record Period
{
    public Period
    (
        string name,
        EventDataTimestamp startTimestamp,
        EventDataTimestamp endTimestamp,
        WinnerRule? winnerRule = null,
        ImmutableHashSet<Period>? children = null
    )
    {
        if (!IsValidPeriodName(name))
        {
            throw new ArgumentException("Invalid name", nameof(name));
        }

        Name = name;
        StartTimestamp = startTimestamp;
        EndTimestamp = endTimestamp;
        WinnerRule = winnerRule;
        Children = children ?? [];
    }

    public string Name { get; }
    public EventDataTimestamp StartTimestamp { get; }
    public EventDataTimestamp EndTimestamp { get; }

    public WinnerRule? WinnerRule { get; }

    public ImmutableHashSet<Period> Children { get; }

    private static bool IsValidPeriodName(string name)
    {
        // Implement validation logic for period names if needed
        return !string.IsNullOrWhiteSpace(name);
    }
}

public record WinnerRule
{
    public WinnerRule(MetricDefinition metric, ScopeType scopeType, OptimumType optimumType)
    {
        Metric = metric;
        ScopeType = scopeType;
        OptimumType = optimumType;
    }

    public MetricDefinition Metric { get; }
    public ScopeType ScopeType { get; }
    public OptimumType OptimumType { get; }
}