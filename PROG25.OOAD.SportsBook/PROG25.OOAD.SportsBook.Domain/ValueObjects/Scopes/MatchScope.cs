using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

public record MatchScope : Scope
{
    public static readonly MatchScope Instance = new();

    private MatchScope()
        : base(ScopeType.Event)
    {
    }

    internal override ScopedEventMetrics ExtractScopedMetrics(EventMetrics state)
    {
        return state.ExtractAllScopes(ScopeType.Event).FirstOrDefault() ?? throw new InvalidOperationException("No match scope metrics found");
    }

    internal override bool IsValidForEventParticipans(ISet<(TeamId TeamId, PlayerId PlayerId)> teamPlayerPairs) => true;
}