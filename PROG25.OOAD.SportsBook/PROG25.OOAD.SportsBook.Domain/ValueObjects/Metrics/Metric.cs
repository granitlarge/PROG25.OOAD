namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

/// <summary>
/// Represents a metric.
/// </summary>
/// <param name="Type">The type of the metric.</param>
public abstract record Metric
(
    MetricType Type
)
{
    /// <summary>
    /// This is an important method that determines whether a supplied metric value is valid for the given metric.
    /// For example: The OverUnderMarket requires a threshold value that must be valid for the metric it uses (e.g., TotalPoints). 
    /// If the metric is TotalPoints, then the threshold value must be a non-negative integer. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public abstract bool IsValidMetricValue(decimal value);

    /// <summary>
    /// Determines whether two instances of the metric value are different from one another.
    /// </summary>
    /// <param name="firstValue"></param>
    /// <param name="secondValue"></param>
    /// <returns></returns>
    public abstract ComparisonResult Compare(decimal firstValue, decimal secondValue);
}