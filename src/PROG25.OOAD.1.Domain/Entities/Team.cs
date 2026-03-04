using System.Security;
using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities;

public class Team
{
    private readonly List<Player> _players = [];

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Team() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

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