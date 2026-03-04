using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Odds;

namespace PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;

public class Outcome
{
    public Outcome(string name, Odds odds)
    {
        if (!CanChangeNameTo(name))
        {
            throw new ArgumentException("Invalid outcome name.", nameof(name));
        }

        Id = new OutcomeId();
        Name = name;
        Odds = odds;
    }

    public OutcomeId Id { get; }
    public string Name { get; private set; }
    public Odds Odds { get; private set; }

    public void ChangeName(string newName)
    {
        if (!CanChangeNameTo(newName))
        {
            throw new ArgumentException("Invalid outcome name.", nameof(newName));
        }

        Name = newName;
    }

    public void ChangeOdds(Odds odds)
    {
        Odds = odds;
    }

    private static bool CanChangeNameTo(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }
}