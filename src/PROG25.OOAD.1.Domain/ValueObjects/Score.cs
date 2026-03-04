namespace PROG25.OOAD.Domain.ValueObjects;

public abstract record Score : IComparable<Score>
{
    public abstract int CompareTo(Score? other);
    public abstract Score Add(Score other);
}
