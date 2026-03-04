namespace PROG25.OOAD.Domain.ValueObjects.Soccer;

public record SoccerMatchTeamState
{
    public SoccerMatchTeamState(TeamId teamId, SoccerScore score, SoccerCorners corners)
    {
        TeamId = teamId;
        Score = score;
        Corners = corners;
    }

    public TeamId TeamId { get; }
    public SoccerScore Score { get; }
    public SoccerCorners Corners { get; }
}