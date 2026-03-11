using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Domain.Entities;

public class Player
{
    public PlayerId Id { get; private set; }
    public string Name { get; private set; }

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