using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;

public class Outcome
{
    public Outcome(string name)
    {
        if (!CanChangeNameTo(name))
        {
            throw new ArgumentException("Invalid outcome name.", nameof(name));
        }

        Id = new OutcomeId();
        Name = name;
    }

    public OutcomeId Id { get; }
    public string Name { get; private set; }

    public void ChangeName(string newName)
    {
        if (!CanChangeNameTo(newName))
        {
            throw new ArgumentException("Invalid outcome name.", nameof(newName));
        }

        Name = newName;
    }

    private static bool CanChangeNameTo(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }
}