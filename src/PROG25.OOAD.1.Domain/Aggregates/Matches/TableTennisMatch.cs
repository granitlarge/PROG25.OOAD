using PROG25.OOAD.Domain.Entities;
using PROG25.OOAD.Domain.ValueObjects;
using MatchType = PROG25.OOAD.Domain.ValueObjects.MatchType;

namespace PROG25.OOAD.Domain.Aggregates.Matches;

public class TableTennisMatch : Match
{
    public TableTennisMatch(MatchId id,
        MatchStatus status,
        DateTimeOffset startTime,
        Team homeTeam,
        Team awayTeam) : base(id, MatchType.TableTennis, status, startTime, [homeTeam, awayTeam])
    {
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
    }

    public Team HomeTeam { get; }
    public Team AwayTeam { get; }

    protected override bool CanCreateMarketInternal(MarketType marketType)
    {
        return marketType switch
        {
            MarketType.Winner => true,
            MarketType.NextTeamToScore => Status == MatchStatus.InProgress,
            MarketType.OverUnder => true,
            _ => false
        };
    }

    public static TableTennisMatch Create(DateTimeOffset startTime, Team homeTeam, Team awayTeam)
    {
        return new TableTennisMatch(new MatchId(), MatchStatus.Scheduled, startTime, homeTeam, awayTeam);
    }
}