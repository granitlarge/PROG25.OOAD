using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Periods;

public abstract record PeriodRules
{
    public abstract Period? GetPeriod(EventData eventData);
}