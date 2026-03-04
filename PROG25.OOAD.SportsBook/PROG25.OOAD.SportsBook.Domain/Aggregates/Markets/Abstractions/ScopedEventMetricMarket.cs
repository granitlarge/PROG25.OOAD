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
        ISet<(TeamId, PlayerId)> teamPlayerPairs,
        ISet<Outcome> outcomes,
        ScopedEventMetricMarketConfiguration configuration
    ) : base(eventId, eventData, configuration, outcomes)
    {
        if (!configuration.Scope.IsValidForEventParticipans(teamPlayerPairs))
        {
            throw new InvalidOperationException("The event statistics do not satisfy the market's scope requirements.");
        }

        Configuration = configuration;
    }

    public override ScopedEventMetricMarketConfiguration Configuration { get; }
}