using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;

/// <summary>
/// Market that settles based on whether a specified scoped metric is equal to an expected value at the time of settlement.
/// For example, this could be used to create a market that settles YES if a team's total goals at the end of the match are equal to a specified value, and NO otherwise.
/// </summary>
public class ExactEventMetricMarket : ScopedEventMetricMarket
{
    internal ExactEventMetricMarket
    (
        Event @event,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        ExactEventMetricMarketConfiguration configuration
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

        YesOutcome = yesOutcome;
        NoOutcome = noOutcome;
        Configuration = configuration;
    }

    public YesNoOutcome YesOutcome { get; }
    public YesNoOutcome NoOutcome { get; }

    public override ExactEventMetricMarketConfiguration Configuration { get; }

    public override MarketStatus Settle(EventStatistics _, Event @event)
    {
        base.Settle(_, @event);
        if (!IsSettleable())
        {
            return Status; // if the base settle method changed the status, we should not proceed with settling this bet
        }

        var scopeState = Configuration.Scope.ExtractScopedStatistics(@event.Statistics);
        var metric = scopeState.Extract(Configuration.Metric.Type);
        var comparisonResult = Configuration.ExpectedValue.Compare(metric);
        var actualMetricValueIsEqualToExpectedValue = ComparisonResult.Equal == comparisonResult;

        var isYes = actualMetricValueIsEqualToExpectedValue;
        Settle(isYes ? YesOutcome.Id : NoOutcome.Id);
        return Status;
    }
}