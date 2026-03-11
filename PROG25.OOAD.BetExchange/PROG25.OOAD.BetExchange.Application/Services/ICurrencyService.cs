using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Application.Services;

public interface ICurrencyService
{
    public Task<Money> Convert(Money money, Currency to);
}