using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities;

public class Player
{
    public PlayerId Id { get; private set; }
    public string Name { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Player() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public Player(PlayerId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Player name cannot be null or empty.", nameof(name));
        }

        Id = id;
        Name = name;
    }
}