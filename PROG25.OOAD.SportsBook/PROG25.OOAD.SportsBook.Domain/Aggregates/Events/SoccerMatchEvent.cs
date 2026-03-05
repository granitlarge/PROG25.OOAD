using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Events;

public class SoccerMatchEvent : Event
{
    private SoccerMatchEvent(Team homeTeam, Team awayTeam, EventData eventData)
        : base(EventTypeEnum.Soccer, new HashSet<Team> { homeTeam, awayTeam }, eventData)
    {
        Data = eventData;
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
    }

    public Team HomeTeam { get; }
    public Team AwayTeam { get; }

    public override EventData Data { get; }

    public static SoccerMatchEvent Create(Team homeTeam, Team awayTeam, EventData eventData)
    {
        return new SoccerMatchEvent(homeTeam, awayTeam, eventData);
    }
}