namespace PROG25.OOAD.SportsBook.Domain.ValueObjects;

public record DefaultIdentifier
{
    public DefaultIdentifier(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Identifier value cannot be empty.", nameof(value));

        Value = value;
    }

    public DefaultIdentifier() : this(Guid.CreateVersion7())
    {
    }

    public Guid Value { get; init; }
}

public record MatchEventId : DefaultIdentifier
{
    public MatchEventId(Guid value) : base(value) { }
    public MatchEventId() : base() { }
}

public record EventId : DefaultIdentifier
{
    public EventId(Guid value) : base(value) { }
    public EventId() : base() { }
}

public record BetId : DefaultIdentifier
{
    public BetId(Guid value) : base(value) { }
    public BetId() : base() { }
}

public record CustomerId : DefaultIdentifier
{
    public CustomerId(Guid value) : base(value) { }
    public CustomerId() : base() { }
}

public record GameId : DefaultIdentifier
{
    public GameId(Guid value) : base(value) { }
    public GameId() : base() { }
}

public record TransactionId : DefaultIdentifier
{
    public TransactionId(Guid value) : base(value) { }
    public TransactionId() : base() { }
}

public record MarketId : DefaultIdentifier
{
    public MarketId(Guid value) : base(value) { }
    public MarketId() : base() { }
}

public record OutcomeId : DefaultIdentifier
{
    public OutcomeId(Guid value) : base(value) { }
    public OutcomeId() : base() { }
}

public record AccountId : DefaultIdentifier
{
    public AccountId(Guid value) : base(value) { }
    public AccountId() : base() { }
}

public record PlayerId : DefaultIdentifier
{
    public PlayerId(Guid value) : base(value) { }
    public PlayerId() : base() { }
}

public record TeamId : DefaultIdentifier
{
    public TeamId(Guid value) : base(value) { }
    public TeamId() : base() { }
}

public record ScopedEventStateId
{
    public ScopedEventStateId(object scopedIdentifier)
    {
        if (!scopedIdentifier.GetType().IsValueType)
            throw new ArgumentException("Scoped identifier must be a value type.", nameof(scopedIdentifier));
        ScopeIdentifier = scopedIdentifier;
    }

    public object ScopeIdentifier { get; }
}

public record EventTypeId : DefaultIdentifier
{
    public EventTypeId(Guid value) : base(value) { }
    public EventTypeId() : base() { }
}

public record BetLegId : DefaultIdentifier
{
    public BetLegId(Guid value) : base(value) { }
    public BetLegId() : base() { }
}

public record PeriodId : DefaultIdentifier
{
    public PeriodId(Guid value) : base(value) { }
    public PeriodId() : base() { }
}