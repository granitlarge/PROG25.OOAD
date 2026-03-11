using PROG25.OOAD.BetExchange.Domain.Aggregates.Bets;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Application.Abstractions.Repositories;

public interface IBetRepository : IRepositoryBase<BetId, Bet>
{

}