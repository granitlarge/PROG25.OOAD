namespace PROG25.OOAD.SportsBook.Domain.ValueObjects;

public record DepositLimit
{
    public DepositLimit(decimal amount, TimeSpan duration)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0");

        Amount = amount;
        Duration = duration;
    }

    public decimal Amount { get; }
    public TimeSpan Duration { get; }
}

public record DepositLimits
{
    private readonly Dictionary<TimeSpan, DepositLimit> _durationToDepositLimit;

    public DepositLimits()
    {
        _durationToDepositLimit = [];
    }

    public DepositLimits(IEnumerable<DepositLimit> depositLimits) : this()
    {
        var durationDuplicates = depositLimits.GroupBy(d => d.Duration).Any(g => g.Count() > 1);
        if (durationDuplicates)
        {
            throw new ArgumentException("Depositlimits contained duplicate entries for the same duration");
        }

        foreach (var depositLimit in depositLimits)
        {
            AddOrReplace(depositLimit);
        }
    }

    public DepositLimit? Get(TimeSpan duration)
    {
        return _durationToDepositLimit.TryGetValue(duration, out var limit) ? limit : null;
    }

    public void AddOrReplace(DepositLimit depositLimit)
    {
        _durationToDepositLimit[depositLimit.Duration] = depositLimit;
    }

    public void Remove(DepositLimit depositLimit)
    {
        _durationToDepositLimit.Remove(depositLimit.Duration);
    }

    public IEnumerable<DepositLimit> GetAll()
    {
        return _durationToDepositLimit.Values;
    }
}