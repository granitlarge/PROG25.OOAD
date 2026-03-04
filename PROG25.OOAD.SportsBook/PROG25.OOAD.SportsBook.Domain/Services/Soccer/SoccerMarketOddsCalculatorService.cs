using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events.Soccer;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.Services.Soccer;

public class SoccerMarketOddsCalculatorService
{
    public (YesNoOutcome YesNoOutcome, YesNoOutcome NoOutcome) CalculateYesNoOutcomes(EventMetricMarketConfiguration marketConfiguration, SoccerMatchEventData soccerMatchEventData)
    {
        return
        (
            new YesNoOutcome(new Odds(1.8m), true),
            new YesNoOutcome(new Odds(1.9m), false)
        );
    }

    public (OverUnderOutcome OverOutcome, OverUnderOutcome UnderOutcome, OverUnderOutcome PushOutcome) CalculateOverUnderOutcomes(OverUnderEventMetricMarketConfiguration marketConfiguration, SoccerMatchEventData soccerMatchEventData)
    {
        return (new OverUnderOutcome(OverUnderOutcomeType.Over, new Odds(1.8m)),
                new OverUnderOutcome(OverUnderOutcomeType.Under, new Odds(1.9m)),
                new OverUnderOutcome(OverUnderOutcomeType.Push, new Odds(1.0m)));
    }
}