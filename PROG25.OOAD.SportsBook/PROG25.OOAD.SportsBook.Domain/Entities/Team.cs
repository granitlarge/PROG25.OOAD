using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Entities;

public class Team
{
    private readonly List<Player> _players = [];

    private Team(TeamId id, string name, List<Player> players)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Team name cannot be null or empty.", nameof(name));
        }

        if (players.Count < 1)
        {
            throw new ArgumentException("A team must have at least one player.", nameof(players));
        }

        Id = id;
        Name = name;
        _players = players;
    }

    public TeamId Id { get; }
    public string Name { get; private set; }
    public IReadOnlyList<Player> Players => _players;
}