using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Application.Abstractions.Repositories;

public interface IMarketRepository : IRepositoryBase<MarketId, EventMetricMarket>
{

}