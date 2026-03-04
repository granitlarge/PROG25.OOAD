namespace PROG25.OOAD.SportsBook.Domain.ValueObjects;

public record SelfExclusion
{
    protected SelfExclusion()
    {

    }

    public SelfExclusion(DateTimeOffset end)
    {
        if (end <= DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("End date must be in the future.", nameof(end));
        }
        End = end;
    }

    public DateTimeOffset End { get; }
    public virtual bool IsActive
    {
        get
        {
            var now = DateTimeOffset.UtcNow;
            return End > now;
        }
    }
}

public record SelfExclusionInactive : SelfExclusion
{
    public static SelfExclusionInactive Instance { get; } = new SelfExclusionInactive();

    private SelfExclusionInactive() : base()
    {

    }

    public override bool IsActive => false;
}