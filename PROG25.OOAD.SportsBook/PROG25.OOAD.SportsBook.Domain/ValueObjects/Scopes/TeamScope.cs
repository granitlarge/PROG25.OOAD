using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

public record TeamScope : Scope
{
    public TeamScope(TeamId teamId)
        : base(ScopeType.Team)
    {
        TeamId = teamId;
    }

    public TeamId TeamId { get; }

    internal override ScopedEventStatistics ExtractScopedStatistics(EventStatistics state)
    {
        return state.ExtractTeamScope(TeamId);
    }

    internal override bool IsValidForEvent(Event match)
    {
        return match.Teams.Any(team => team.Id == TeamId);;
    }
}