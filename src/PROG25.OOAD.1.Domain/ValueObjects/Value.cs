namespace PROG25.OOAD.Domain.ValueObjects;

public record Value
{
    private readonly byte[] _data;
    public ReadOnlySpan<byte> Data => _data;
    public ValueType Type { get; }

    public Value(byte[] data, ValueType type)
    {
        _data = data;
        Type = type;
    }

    public Guid AsGuid()
    {
        if (Type != ValueType.Guid)
            throw new InvalidOperationException("Value is not of type Guid.");
        return new Guid(_data);
    }

    public string AsString()
    {
        if (Type != ValueType.String)
            throw new InvalidOperationException("Value is not of type String.");
        return System.Text.Encoding.UTF8.GetString(_data);
    }

    public int AsInteger()
    {
        if (Type != ValueType.Integer)
            throw new InvalidOperationException("Value is not of type Integer.");
        return BitConverter.ToInt32(_data, 0);
    }
}

public enum ValueType
{
    Guid,
    String,
    Integer,
}