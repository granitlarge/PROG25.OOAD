namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

/// <summary>
/// Represents a metric that must be different from a reference value by at least a specified fault tolerance, with a specified difference type (greater than, less than, etc.).
/// This captures a common pattern. For example, the "points scored", the "corners scored", the "time elapsed" metric must be greater or equal to 0 in most sports.
/// I need a better name for this class: 
/// </summary>
public record ReferenceValueBasedMetric : Metric
{
    public static readonly ReferenceValueBasedMetric NonNegativePoints = new(MetricType.Points, DifferenceType.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);
    public static readonly ReferenceValueBasedMetric Corners = new(MetricType.Corners, DifferenceType.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);
    public static readonly ReferenceValueBasedMetric YellowCards = new(MetricType.YellowCards, DifferenceType.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);
    public static readonly ReferenceValueBasedMetric RedCards = new(MetricType.RedCards, DifferenceType.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);
    public static readonly ReferenceValueBasedMetric ElapsedMatchTimeSeconds = new(MetricType.ElapsedMatchTimeSeconds, DifferenceType.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);
    public static readonly ReferenceValueBasedMetric ElapsedActualTimeSeconds = new(MetricType.ElapsedActualTimeSeconds, DifferenceType.GreaterThan, ReferenceValueDecimalComparer.NegativeOneZeroTolerance);

    public ReferenceValueBasedMetric
    (
        MetricType type,
        DifferenceType differenceType,
        ReferenceValueDecimalComparer referenceValueDecimalChanged) : base(type)
    {
        DifferenceType = differenceType;
        ReferenceValueDecimalChanged = referenceValueDecimalChanged;
    }


    public DifferenceType DifferenceType { get; }
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
            ComparisonResult.GreaterThan => DifferenceType == DifferenceType.GreaterThan,
            ComparisonResult.LessThan => DifferenceType == DifferenceType.LessThan,
            _ => throw new InvalidOperationException("Unexpected comparison result.")
        };
    }
}

public enum DifferenceType
{
    GreaterThan,
    LessThan
}