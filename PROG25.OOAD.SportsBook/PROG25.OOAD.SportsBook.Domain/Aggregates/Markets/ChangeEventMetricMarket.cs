using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;

/// <summary>
/// Market that settles based on whether a specified scoped metric has changed in a certain way compared to a reference value at the time of settlement.
/// </summary>
public class ChangeEventMetricMarket : ScopedEventMetricMarket
{
    internal ChangeEventMetricMarket
    (
        Event @event,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        ChangeEventMetricMarketConfiguration configuration
    ) : base(@event, new HashSet<Outcome> { yesOutcome, noOutcome }, configuration)
    {
        if (!yesOutcome.IsYes)
        {
            throw new ArgumentException("yesOutcome must have IsYes set to true.", nameof(yesOutcome));
        }
        if (noOutcome.IsYes)
        {
            throw new ArgumentException("noOutcome must have IsYes set to false.", nameof(noOutcome));
        }
        Configuration = configuration;
        YesOutcome = yesOutcome;
        NoOutcome = noOutcome;
    }

    public YesNoOutcome YesOutcome { get; }
    public YesNoOutcome NoOutcome { get; }

    public override ChangeEventMetricMarketConfiguration Configuration { get; }

    public override MarketStatus Settle(EventStatistics previousEventStats, Event @event)
    {
        base.Settle(previousEventStats, @event);
        if (!IsSettleable())
        {
            return Status;
        }

        var currentMetricValue = Configuration
        .Scope
        .ExtractScopedStatistics(@event.Statistics)
        .Extract(Configuration.Metric.Type);

        var compareResult = Configuration.Metric.Compare(Configuration.ReferenceValue, currentMetricValue);
        var isYes = compareResult == Configuration.ChangeType;
        Settle(isYes ? YesOutcome.Id : NoOutcome.Id);

        return Status;
    }
}