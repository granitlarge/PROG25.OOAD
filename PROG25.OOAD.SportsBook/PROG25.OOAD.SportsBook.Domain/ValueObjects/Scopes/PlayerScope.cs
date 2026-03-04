using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

public record PlayerScope : Scope
{
    public PlayerScope(PlayerId playerId)
        : base(ScopeType.Player)
    {
        PlayerId = playerId;
    }

    public PlayerId PlayerId { get; }

    internal override ScopedEventStatistics ExtractScopedStatistics(EventStatistics state)
    {
        return state.ExtractPlayerScope(PlayerId);
    }

    internal override bool IsValidForEvent(Event match)
    {
        var hasPlayer = match.Teams.Any(team => team.Players.Any(player => player.Id == PlayerId));
        return hasPlayer;
    }
}