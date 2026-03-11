using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;

public abstract class ScopedEventMetricMarket : EventMetricMarket
{
    protected ScopedEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        ScopedEventMetricMarketConfiguration configuration,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome
    ) : base(eventId, eventData, configuration, yesOutcome, noOutcome)
    {
        Configuration = configuration;
    }

    public override ScopedEventMetricMarketConfiguration Configuration { get; }
}