using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Odds;

namespace PROG25.OOAD.SportsBook.Domain.Services.Soccer;

public class SoccerMarketOddsCalculatorService
{
    public (YesNoOutcome YesNoOutcome, YesNoOutcome NoOutcome) CalculateYesNoOutcomes(EqualScopeEventMetricMetricMarketConfiguration config, EventData eventData)
    {
        return
        (
            new YesNoOutcome(new Odds(1.8m), true),
            new YesNoOutcome(new Odds(1.9m), false)
        );
    }

    public (YesNoOutcome YesNoOutcome, YesNoOutcome NoOutcome) CalculateYesNoOutcomes(OptimalScopedEventMetricMarketConfiguration config, EventData eventData)
    {
        return
        (
            new YesNoOutcome(new Odds(1.8m), true),
            new YesNoOutcome(new Odds(1.9m), false)
        );
    }

    public (YesNoOutcome YesNoOutcome, YesNoOutcome NoOutcome) CalculateYesNoOutcomes(ComparisonScopedEventMetricMarketConfiguration config, EventData eventData)
    {
        return
        (
            new YesNoOutcome(new Odds(1.8m), true),
            new YesNoOutcome(new Odds(1.9m), false)
        );
    }
}