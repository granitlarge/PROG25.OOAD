namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;

public record EventScope : Scope
{
    public static readonly EventScope Instance = new();

    private EventScope()
        : base(ScopeType.Event)
    {
    }
}