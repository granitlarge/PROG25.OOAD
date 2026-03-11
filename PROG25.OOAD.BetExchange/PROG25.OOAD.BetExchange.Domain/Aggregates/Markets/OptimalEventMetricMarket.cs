using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets;

/// <summary>
/// Market that settles based on whether a specific dimension combination's metric value is optimal (maximum or minimum) compared to all other dimension combinations for the same metric.
/// </summary>
public class OptimalEventMetricMarket : EventMetricMarket
{
    internal OptimalEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        OptimalDimensionEventMetricMarketConfiguration configuration
    ) : base(eventId, eventData, configuration, new HashSet<Outcome> { yesOutcome, noOutcome })
    {
        if (!yesOutcome.IsYes)
        {
            throw new ArgumentException("YesOutcome must have IsYes = true");
        }

        if (noOutcome.IsYes)
        {
            throw new ArgumentException("NoOutcome must have IsYes = false");
        }

        Configuration = configuration;
        YesOutcome = yesOutcome;
        NoOutcome = noOutcome;
    }

    public override OptimalDimensionEventMetricMarketConfiguration Configuration { get; }
    public YesNoOutcome YesOutcome { get; }
    public YesNoOutcome NoOutcome { get; }

    public override SettlementAttemptStatus TrySettle(EventData eventData)
    {
        var settlementAttemptStatus = base.TrySettle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus;
        }

        var specificDimensionCombinationMetric = eventData.Metrics.Extract([Configuration.Dimension], Configuration.Metric);

        var metricsForAllDimensionsExceptSpecificOne = eventData.Metrics
        .ExtractAll([.. Configuration.Dimension.Value.Select(kv => kv.Key).Distinct()], Configuration.Metric)
        .Where(mv => mv.Query != Configuration.Dimension)
        .ToList();

        var isOptimal = Configuration.OptimumType switch
        {
            OptimumType.Maximum => metricsForAllDimensionsExceptSpecificOne.Max(e => e.Value) < specificDimensionCombinationMetric,
            OptimumType.Minimum => metricsForAllDimensionsExceptSpecificOne.Min(e => e.Value) > specificDimensionCombinationMetric,
            _ => throw new InvalidOperationException("Unsupported optimality type.")
        };

        Settle(isOptimal ? YesOutcome.Id : NoOutcome.Id);
        return SettlementAttemptStatus.Completed;
    }
}