using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Application.Abstractions.Repositories;

public interface IMarketRepository : IRepositoryBase<MarketId, EventMetricMarket>
{

}