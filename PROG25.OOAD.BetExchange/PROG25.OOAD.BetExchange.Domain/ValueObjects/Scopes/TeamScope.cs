namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;

public record TeamScope : Scope
{
    public TeamScope(TeamId teamId)
        : base(ScopeType.Team)
    {
        TeamId = teamId;
    }

    public TeamId TeamId { get; }
}