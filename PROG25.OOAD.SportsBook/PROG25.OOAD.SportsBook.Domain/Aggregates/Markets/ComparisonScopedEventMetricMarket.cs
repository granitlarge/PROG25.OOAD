using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;

public class ComparisonScopedEventMetricMarket : ScopedEventMetricMarket
{
    internal ComparisonScopedEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        ISet<(TeamId, PlayerId)> teamPlayerPairs,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        ComparisonScopedEventMetricMarketConfiguration configuration
    ) : base(eventId, eventData, teamPlayerPairs, new HashSet<Outcome> { yesOutcome, noOutcome }, configuration)
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

    public override ComparisonScopedEventMetricMarketConfiguration Configuration { get; }

    public override SettlementAttemptStatus Settle(EventData eventData)
    {
        var settlementAttemptStatus = base.Settle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus;
        }

        var currentMetricValue = Configuration
        .Scope
        .ExtractScopedMetrics(eventData.Metrics)
        .Extract(Configuration.Metric.Type);

        var compareResult = Configuration.Metric.Compare(Configuration.ReferenceValue, currentMetricValue);
        var isYes = compareResult == Configuration.ChangeType;
        Settle(isYes ? YesOutcome.Id : NoOutcome.Id);

        return SettlementAttemptStatus.Completed;
    }
}