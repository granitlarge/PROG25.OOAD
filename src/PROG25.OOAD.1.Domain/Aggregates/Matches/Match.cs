using PROG25.OOAD.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.Domain.Entities;
using PROG25.OOAD.Domain.ValueObjects;
using MatchType = PROG25.OOAD.Domain.ValueObjects.MatchType;

namespace PROG25.OOAD.Domain.Aggregates.Matches;

public abstract class Match
{
    protected Match(MatchId id, MatchType matchType, MatchStatus status, DateTimeOffset startTime, List<Team> teams)
    {
        if (teams.Count == 0)
        {
            throw new ArgumentException("A match must have at least one team.");
        }

        Id = id;
        StartTime = startTime;
        Type = matchType;
        Status = status;
        Teams = teams;
    }

    public MatchId Id { get; }
    public DateTimeOffset StartTime { get; }
    public MatchType Type { get; }
    public MatchStatus Status { get; private set; }
    public List<Team> Teams { get; }

    public void Start()
    {
        if (Status != MatchStatus.Scheduled)
            throw new InvalidOperationException("Match can only be started from a scheduled state.");

        Status = MatchStatus.InProgress;
    }

    public void End()
    {
        if (Status != MatchStatus.InProgress)
            throw new InvalidOperationException("Match can only be ended from an in-progress state.");

        Status = MatchStatus.Ended;
    }

    public void Cancel()
    {
        if (Status == MatchStatus.Ended)
            throw new InvalidOperationException("Ended match cannot be cancelled.");

        Status = MatchStatus.Cancelled;
    }

    internal bool CanCreateMarket(MarketType marketType)
    {
        return (Status == MatchStatus.Scheduled || Status == MatchStatus.InProgress) && CanCreateMarketInternal(marketType);
    }

    protected abstract bool CanCreateMarketInternal(MarketType marketType);
}