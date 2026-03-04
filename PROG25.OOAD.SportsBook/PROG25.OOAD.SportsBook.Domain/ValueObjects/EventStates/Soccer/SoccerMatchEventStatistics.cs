using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates.Soccer;

public record SoccerMatchEventStatistics : EventStatistics
{
    private readonly ImmutableHashSet<Metric> _supportedMetrics = new HashSet<Metric>
    {
        ReferenceValueBasedMetric.NonNegativePoints,
        ReferenceValueBasedMetric.Corners,
        ReferenceValueBasedMetric.YellowCards,
        ReferenceValueBasedMetric.RedCards
    }.ToImmutableHashSet();

    public SoccerMatchEventStatistics
    (
        EventId eventId,
        TeamId homeTeamId,
        TeamId awayTeamId,
        EventStatus status,
        TimeSpan elapsedMatchTime,
        TimeSpan elapsedActualTime,
        Dictionary<(PlayerId PlayerId, TeamId TeamId), uint> playerPoints,
        Dictionary<(PlayerId PlayerId, TeamId TeamId), uint> playerYellowCards,
        Dictionary<(PlayerId PlayerId, TeamId TeamId), uint> playerRedCards,
        Dictionary<(PlayerId PlayerId, TeamId TeamId), uint> playerCorners
    ) : base(eventId, elapsedMatchTime, elapsedActualTime, status)
    {
        HomeTeamId = homeTeamId;
        AwayTeamId = awayTeamId;
        PlayerPoints = playerPoints;
        PlayerYellowCards = playerYellowCards;
        PlayerRedCards = playerRedCards;
        PlayerCorners = playerCorners;
    }

    public TeamId HomeTeamId { get; private set; }
    public TeamId AwayTeamId { get; private set; }

    public uint HomeTeamPoints => (uint)PlayerPoints.Where(kv => kv.Key.TeamId == HomeTeamId).Sum(kv => kv.Value);
    public uint AwayTeamPoints => (uint)PlayerPoints.Where(kv => kv.Key.TeamId == AwayTeamId).Sum(kv => kv.Value);

    public Dictionary<(PlayerId PlayerId, TeamId TeamId), uint> PlayerPoints { get; private set; }
    public Dictionary<(PlayerId PlayerId, TeamId TeamId), uint> PlayerYellowCards { get; private set; }
    public Dictionary<(PlayerId PlayerId, TeamId TeamId), uint> PlayerRedCards { get; private set; }
    public Dictionary<(PlayerId PlayerId, TeamId TeamId), uint> PlayerCorners { get; private set; }

    protected override ImmutableHashSet<Metric> SupportedMetrics => _supportedMetrics;

    internal override List<ScopedEventStatistics> ExtractAllScopes(ScopeType scopeType)
    {
        switch (scopeType)
        {
            case ScopeType.Match:
                return [ExtractEventScope()];
            case ScopeType.Team:
                return
                [
                    ExtractTeamScope(HomeTeamId),
                    ExtractTeamScope(AwayTeamId)
                ];
            case ScopeType.Player:
                var playerIds = PlayerPoints.Select(kv => kv.Key.PlayerId)
                    .Union(PlayerYellowCards.Select(kv => kv.Key.PlayerId))
                    .Union(PlayerRedCards.Select(kv => kv.Key.PlayerId))
                    .Union(PlayerCorners.Select(kv => kv.Key.PlayerId))
                    .Distinct();
                return playerIds.Select(ExtractPlayerScope).ToList();
            default:
                throw new NotSupportedException($"Scope type {scopeType} is not supported in SoccerMatchState.");
        }
    }

    internal override ScopedEventStatistics ExtractEventScope() =>
        new ScopedSoccerMatchEventState
        (
            0,
            ElapsedMatchTime,
            ElapsedActualTime,
            HomeTeamPoints + AwayTeamPoints,
            (uint)PlayerYellowCards.Sum(x => x.Value),
            (uint)PlayerRedCards.Sum(x => x.Value),
            (uint)PlayerCorners.Sum(x => x.Value)
        );

    internal override ScopedEventStatistics ExtractPlayerScope(PlayerId playerId)
    {
        return new ScopedSoccerMatchEventState
        (
            playerId,
            ElapsedMatchTime,
            ElapsedActualTime,
            PlayerPoints.FirstOrDefault(kv => kv.Key.PlayerId == playerId).Value,
            PlayerYellowCards.FirstOrDefault(kv => kv.Key.PlayerId == playerId).Value,
            PlayerRedCards.FirstOrDefault(kv => kv.Key.PlayerId == playerId).Value,
            PlayerCorners.FirstOrDefault(kv => kv.Key.PlayerId == playerId).Value
        );
    }

    internal override ScopedEventStatistics ExtractTeamScope(TeamId teamId)
    {
        return new ScopedSoccerMatchEventState
        (
            teamId,
            ElapsedMatchTime,
            ElapsedActualTime,
            (uint)PlayerPoints.Where(kv => kv.Key.TeamId == teamId).Sum(kv => kv.Value),
            (uint)PlayerYellowCards.Where(kv => kv.Key.TeamId == teamId).Sum(kv => kv.Value),
            (uint)PlayerRedCards.Where(kv => kv.Key.TeamId == teamId).Sum(kv => kv.Value),
            (uint)PlayerCorners.Where(kv => kv.Key.TeamId == teamId).Sum(kv => kv.Value)
        );
    }

    public override void UpdateMetric(TeamId teamId, PlayerId playerId, MetricType metricType, decimal newValue)
    {
        switch (metricType)
        {
            case MetricType.Points:
                var scoreEntry = PlayerPoints.Single(kv => kv.Key.PlayerId == playerId && kv.Key.TeamId == teamId);
                PlayerPoints[scoreEntry.Key] = (uint)newValue;
                break;
            case MetricType.YellowCards:
                var yellowCardEntry = PlayerYellowCards.Single(kv => kv.Key.PlayerId == playerId && kv.Key.TeamId == teamId);
                PlayerYellowCards[yellowCardEntry.Key] = (uint)newValue;
                break;
            case MetricType.RedCards:
                var redCardEntry = PlayerRedCards.Single(kv => kv.Key.PlayerId == playerId && kv.Key.TeamId == teamId);
                PlayerRedCards[redCardEntry.Key] = (uint)newValue;
                break;
            case MetricType.Corners:
                var cornerEntry = PlayerCorners.Single(kv => kv.Key.PlayerId == playerId && kv.Key.TeamId == teamId);
                PlayerCorners[cornerEntry.Key] = (uint)newValue;
                break;
            default:
                base.UpdateMetric(teamId, playerId, metricType, newValue);
                break;
        }
    }
}