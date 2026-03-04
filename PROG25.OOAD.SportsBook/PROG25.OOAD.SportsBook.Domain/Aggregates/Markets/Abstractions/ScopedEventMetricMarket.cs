using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;

public abstract class ScopedEventMetricMarket : EventMetricMarket
{
    protected ScopedEventMetricMarket
    (
        Event @event,
        ISet<Outcome> outcomes,
        ScopedEventMetricMarketConfiguration configuration
    ) : base(@event, configuration, outcomes)
    {
        if (!configuration.Scope.IsValidForEvent(@event))
        {
            throw new InvalidOperationException("The event statistics do not satisfy the market's scope requirements.");
        }

        Configuration = configuration;
    }

    public override ScopedEventMetricMarketConfiguration Configuration { get; }
}