using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.Outcome;

public abstract class Outcome
{
    protected Outcome(OutcomeId id, string name, Odds odds)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Outcome name cannot be null or empty.", nameof(name));

        Id = id;
        Name = name;
        Odds = odds;
    }

    public OutcomeId Id { get; }

    public string Name { get; }

    public Odds Odds { get; private set; }

    internal void ChangeOdds(Odds newOdds)
    {
        Odds = newOdds;
    }
}