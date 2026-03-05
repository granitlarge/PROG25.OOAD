using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;

public abstract class ScopedEventMetricMarket : EventMetricMarket
{
    protected ScopedEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        ScopedEventMetricMarketConfiguration configuration,
        ISet<Outcome> outcomes
    ) : base(eventId, eventData, configuration, outcomes)
    {
        Configuration = configuration;
    }

    public override ScopedEventMetricMarketConfiguration Configuration { get; }
}