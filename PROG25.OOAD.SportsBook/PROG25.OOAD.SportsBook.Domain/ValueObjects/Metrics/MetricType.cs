namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

public enum MetricType
{
    #region General metrics

    ElapsedMatchTimeSeconds, // How many seconds have elapsed since the start of the match, excluding stoppage time
    ElapsedActualTimeSeconds, // How many seconds have actually elapsed since the start of the match, including stoppage time
    Points,
    Time,
    Distance,

    Period, // e.g., quarter, half, set, etc.

    #endregion

    #region Soccer-specific metrics

    Corners,
    YellowCards,
    RedCards,

    #endregion
}