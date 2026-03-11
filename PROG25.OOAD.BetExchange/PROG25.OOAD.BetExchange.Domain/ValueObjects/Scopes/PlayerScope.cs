namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;

public record PlayerScope : Scope
{
    public PlayerScope(TeamId teamId, PlayerId playerId)
        : base(ScopeType.Player)
    {
        TeamId = teamId;
        PlayerId = playerId;
    }

    public TeamId TeamId { get; }
    public PlayerId PlayerId { get; }
}