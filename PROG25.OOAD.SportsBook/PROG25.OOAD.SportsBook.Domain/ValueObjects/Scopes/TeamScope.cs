using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

public record TeamScope : Scope
{
    public TeamScope(TeamId teamId)
        : base(ScopeType.Team)
    {
        TeamId = teamId;
    }

    public TeamId TeamId { get; }

    internal override ScopedEventMetrics ExtractScopedMetrics(EventMetrics state)
    {
        return state.ExtractTeamScope(TeamId);
    }

    internal override bool IsValidForEventParticipans(ISet<(TeamId TeamId, PlayerId PlayerId)> teamPlayerPairs)
    {
        return teamPlayerPairs.Any(pair => pair.TeamId == TeamId);
    }
}