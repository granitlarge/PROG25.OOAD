using PROG25.OOAD.SportsBook.Domain.Aggregates.Bets;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Application.Abstractions.Repositories;

public interface IBetRepository : IRepositoryBase<BetId, Bet>
{

}