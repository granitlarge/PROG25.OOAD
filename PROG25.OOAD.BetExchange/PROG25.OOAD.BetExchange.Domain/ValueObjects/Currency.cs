namespace PROG25.OOAD.BetExchange.Domain.ValueObjects;

public record Currency(string Code)
{
    public static readonly Currency Euro = new("EUR");
}