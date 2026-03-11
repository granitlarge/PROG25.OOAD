using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Domain.Entities;

public class Team
{
    private readonly List<Player> _players = [];

    private Team(string name, List<Player> players)
    {
        EnsureIsValidName(name);
        if (players.Count < 1)
        {
            throw new ArgumentException("A team must have at least one player.", nameof(players));
        }

        Id = new TeamId();
        Name = name;
        _players = players;
    }

    public TeamId Id { get; }
    public string Name { get; private set; }
    public IReadOnlyList<Player> Players => _players;

    internal void ChangeName(string newName)
    {
        EnsureIsValidName(newName);
        Name = newName;
    }

    private static void EnsureIsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Team name cannot be null or empty.", nameof(name));
        }
    }
}