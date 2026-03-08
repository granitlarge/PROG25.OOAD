using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Periods;

public abstract record PeriodRules
{
    public abstract Period? GetPeriod(EventData eventData);
}