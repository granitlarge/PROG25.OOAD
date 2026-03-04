using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;

public abstract class EventMetricMarket : Market
{
    protected EventMetricMarket
    (
        Event @event,
        EventMetricMarketConfiguration configuration,
        ISet<Outcome> outcomes
    ) : base(@event.Id, configuration, outcomes)
    {
        if (configuration.Timestamp.HasOccured(@event.Statistics))
        {
            throw new InvalidOperationException("Cannot create an event metric market for an event that has already passed the market's timestamp");
        }

        if (!@event.Statistics.IsSupportedMetric(configuration.Metric.Type))
        {
            throw new InvalidOperationException($"The event does not support the metric type {configuration.Metric.Type} required by the market configuration.");
        }

        Configuration = configuration;
    }

    public override EventMetricMarketConfiguration Configuration { get; }

    public override MarketStatus Settle(EventStatistics previousEventStats, Event @event)
    {
        base.Settle(previousEventStats, @event);
        if (!IsSettleable())
        {
            return Status;
        }

        if (!Configuration.Timestamp.HasOccured(previousEventStats, @event.Statistics))
        {
            if (@event.Statistics.Status == EventStatus.Finished)
            {
                Settle(null); // if the event finished without the timestamp occurring, the bet is lost, but no winning outcome can be determined, so we settle with null winning outcome id
                return Status;
            }
        }

        return Status;
    }
}