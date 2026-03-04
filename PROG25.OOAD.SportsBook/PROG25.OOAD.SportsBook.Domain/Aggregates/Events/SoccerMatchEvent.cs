using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates.Soccer;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Events;

using EventType = ValueObjects.EventType;

public class SoccerMatchEvent : Event
{
    private SoccerMatchEvent(DateTimeOffset startDate, Team homeTeam, Team awayTeam, SoccerMatchEventStatistics statistics)
        : base(EventType.Soccer, [homeTeam, awayTeam], statistics, startDate)
    {
        Statistics = statistics;
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
    }

    public Team HomeTeam { get; }
    public Team AwayTeam { get; }

    public override SoccerMatchEventStatistics Statistics { get; }

    public static SoccerMatchEvent Create(DateTimeOffset startDate, Team homeTeam, Team awayTeam, SoccerMatchEventStatistics statistics)
    {
        return new SoccerMatchEvent(startDate, homeTeam, awayTeam, statistics);
    }
}