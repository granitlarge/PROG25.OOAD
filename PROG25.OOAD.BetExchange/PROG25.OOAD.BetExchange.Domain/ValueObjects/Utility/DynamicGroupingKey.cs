using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.Extensions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Utility;

public class DynamicGroupingKey : IEquatable<DynamicGroupingKey>
{
    private readonly ImmutableDictionary<string, object> _values;
    private int? _hashCode;

    public DynamicGroupingKey(ImmutableDictionary<string, object> values)
    {
        _values = values;
    }

    public ImmutableDictionary<string, object> Values => _values;

    public bool Equals(DynamicGroupingKey? other)
    {
        if (other is null)
        {
            return false;
        }

        return
            _values.Count == other._values.Count &&
            _values.All(other._values.Contains) &&
            other._values.All(_values.Contains);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as DynamicGroupingKey);
    }

    public override int GetHashCode()
    {
        return _hashCode ??= _values.CalculateHashCode();
    }

    public static bool operator ==(DynamicGroupingKey left, DynamicGroupingKey right)
    {
        return left.GetHashCode() == right.GetHashCode() && left.Equals(right);
    }

    public static bool operator !=(DynamicGroupingKey left, DynamicGroupingKey right)
    {
        return !(left == right);
    }
}
