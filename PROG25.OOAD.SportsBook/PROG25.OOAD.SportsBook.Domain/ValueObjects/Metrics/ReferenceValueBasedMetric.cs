namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

/// <summary>
/// Represents a metric that must be different from a reference value by at least a specified fault tolerance, with a specified difference type (greater than, less than, etc.).
/// This captures a common pattern. For example, the "points scored", the "corners scored", the "time elapsed" metric must be greater or equal to 0 in most sports.
/// I need a better name for this class: 
/// </summary>
public record ReferenceValueBasedMetric : Metric
{
    public static readonly ReferenceValueBasedMetric NonNegativePoints = new(MetricType.Points, ComparisonResult.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);
    public static readonly ReferenceValueBasedMetric Corners = new(MetricType.Corners, ComparisonResult.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);
    public static readonly ReferenceValueBasedMetric YellowCards = new(MetricType.YellowCards, ComparisonResult.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);
    public static readonly ReferenceValueBasedMetric RedCards = new(MetricType.RedCards, ComparisonResult.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);
    public static readonly ReferenceValueBasedMetric ElapsedMatchTimeSeconds = new(MetricType.ElapsedMatchTimeSeconds, ComparisonResult.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);
    public static readonly ReferenceValueBasedMetric ElapsedActualTimeSeconds = new(MetricType.ElapsedActualTimeSeconds, ComparisonResult.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);

    public ReferenceValueBasedMetric
    (
        MetricType type,
        ComparisonResult comparisonResult,
        ReferenceValueDecimalComparer referenceValueDecimalChanged) : base(type)
    {
        if (comparisonResult == ComparisonResult.Equal)
        {
            throw new ArgumentException("DifferenceType cannot be Equal for a ReferenceValueBasedMetric, as the metric value must be different from the reference value.", nameof(comparisonResult));
        }

        DifferenceType = comparisonResult;
        ReferenceValueDecimalChanged = referenceValueDecimalChanged;
    }

    public ComparisonResult DifferenceType { get; }
    public ReferenceValueDecimalComparer ReferenceValueDecimalChanged { get; }

    public override ComparisonResult Compare(decimal firstValue, decimal secondValue)
    {
        if (!IsValidMetricValue(firstValue) || !IsValidMetricValue(secondValue))
        {
            throw new ArgumentException("One or both metric values are invalid for this metric.");
        }

        return ReferenceValueDecimalChanged.FaultTolerance.Compare(firstValue, secondValue);
    }

    public override bool IsValidMetricValue(decimal value)
    {
        var result = ReferenceValueDecimalChanged.Compare(value);
        return result switch
        {
            ComparisonResult.Equal => false,
            ComparisonResult.GreaterThan => DifferenceType == ComparisonResult.GreaterThan,
            ComparisonResult.LessThan => DifferenceType == ComparisonResult.LessThan,
            _ => throw new InvalidOperationException("Unexpected comparison result.")
        };
    }
}