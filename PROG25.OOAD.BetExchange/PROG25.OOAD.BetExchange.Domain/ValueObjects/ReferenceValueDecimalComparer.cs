namespace PROG25.OOAD.BetExchange.Domain.ValueObjects;

/// <summary>
/// A helper class for comparing decimal values against a reference value with a specified fault tolerance.
/// </summary>
public record ReferenceValueDecimalComparer
{
    public static readonly ReferenceValueDecimalComparer NegativeOneZeroTolerance = new(-1, FaultTolerance.Zero);

    public ReferenceValueDecimalComparer(decimal referenceValue, FaultTolerance faultTolerance)
    {
        ReferenceValue = referenceValue;
        FaultTolerance = faultTolerance;
    }

    public decimal ReferenceValue { get; }
    public FaultTolerance FaultTolerance { get; }

    public ComparisonResult Compare(decimal currentValue)
    {
        return FaultTolerance.Compare(currentValue, ReferenceValue);
    }
}