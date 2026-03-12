using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets;

/// <summary>
/// Market that settles based on whether a specific dimension combination's metric value is optimal (maximum or minimum) compared to all other dimension combinations for the same metric.
/// </summary> 
public class OptimalDimensionEventMetricMarket : EventMetricMarket
{
    internal OptimalDimensionEventMetricMarket
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

        var metricValues = eventData.Metrics.GetByDefinition(Configuration.MetricDefinition);

        // Fetch the metric value for the specific dimension
        var specificDimensionMetricValue = Configuration.MetricDefinition.Aggregate(Configuration.MetricDefinition.Filter([Configuration.Dimension], metricValues));

        // Fetch the metric value for all other combinations of the dimension
        var metricsForAllDimensionsExceptSpecificOne = Configuration.MetricDefinition
            .GroupBy([.. Configuration.Dimension.Value.Select(kv => kv.Key).Distinct()], metricValues)
            .Select(group => (group.Key, AggregatedMetricValue: Configuration.MetricDefinition.Aggregate(group.Values)))
#warning does this comparison work?
            .Where(group => group.Key != Configuration.Dimension)
            .Select(group => group.AggregatedMetricValue)
            .ToList();

        var isOptimal = Configuration.OptimumType switch
        {
            OptimumType.Maximum => metricsForAllDimensionsExceptSpecificOne.Max() < specificDimensionMetricValue,
            OptimumType.Minimum => metricsForAllDimensionsExceptSpecificOne.Min() > specificDimensionMetricValue,
            _ => throw new InvalidOperationException("Unsupported optimality type.")
        };

        Settle(isOptimal ? YesOutcome.Id : NoOutcome.Id);
        return SettlementAttemptStatus.Completed;
    }
}