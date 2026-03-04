using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

public abstract record Scope
{
    protected Scope(ScopeType type)
    {
        Type = type;
    }

    public ScopeType Type { get; }

    internal abstract ScopedEventStatistics ExtractScopedStatistics(EventStatistics state);
    internal abstract bool IsValidForEvent(Event @event);
}