using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;

public abstract class EventMetricMarket : Market
{
    protected EventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        EventMetricMarketConfiguration configuration,
        ISet<Outcome> outcomes
    ) : base(eventId, configuration, outcomes)
    {
        if (configuration.Timestamp.HasOccurred(eventData))
        {
            throw new InvalidOperationException("Cannot create an event metric market for an event that has already passed the market's timestamp");
        }

        if (!eventData.Metrics.IsSupportedMetric(configuration.Metric.Type))
        {
            throw new InvalidOperationException($"The event does not support the metric type {configuration.Metric.Type} required by the market configuration.");
        }

        Configuration = configuration;
    }

    public override EventMetricMarketConfiguration Configuration { get; }
}