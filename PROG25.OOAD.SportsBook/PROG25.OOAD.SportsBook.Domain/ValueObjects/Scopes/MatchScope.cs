using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

public record MatchScope : Scope
{
    public static readonly MatchScope Instance = new();

    private MatchScope()
        : base(ScopeType.Match)
    {
    }

    internal override ScopedEventStatistics ExtractScopedStatistics(EventStatistics state)
    {
        return state.ExtractEventScope();
    }

    internal override bool IsValidForEvent(Event m)
    {
        return true;
    }
}