using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

public record PlayerScope : Scope
{
    public PlayerScope(PlayerId playerId)
        : base(ScopeType.Player)
    {
        PlayerId = playerId;
    }

    public PlayerId PlayerId { get; }

}