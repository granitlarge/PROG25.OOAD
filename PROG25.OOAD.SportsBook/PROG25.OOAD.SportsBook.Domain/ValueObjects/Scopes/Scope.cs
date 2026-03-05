using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

public abstract record Scope
{
    protected Scope(ScopeType type)
    {
        Type = type;
    }

    public ScopeType Type { get; }
}