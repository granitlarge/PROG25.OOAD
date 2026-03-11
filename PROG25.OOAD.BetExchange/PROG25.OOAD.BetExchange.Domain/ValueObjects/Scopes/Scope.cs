namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;

public abstract record Scope
{
    protected Scope(ScopeType type)
    {
        Type = type;
    }

    public ScopeType Type { get; }
}