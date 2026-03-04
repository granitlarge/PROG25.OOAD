using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events.Soccer;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Events;

using EventType = ValueObjects.EventType;

public class SoccerMatchEvent : Event
{
    private SoccerMatchEvent(Team homeTeam, Team awayTeam, SoccerMatchEventData eventData)
        : base(EventType.Soccer, new HashSet<Team> { homeTeam, awayTeam }, eventData)
    {
        Data = eventData;
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
    }

    public Team HomeTeam { get; }
    public Team AwayTeam { get; }

    public override SoccerMatchEventData Data { get; }

    public static SoccerMatchEvent Create(Team homeTeam, Team awayTeam, SoccerMatchEventData eventData)
    {
        return new SoccerMatchEvent(homeTeam, awayTeam, eventData);
    }
}