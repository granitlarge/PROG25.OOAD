using PROG25.OOAD.BetExchange.Domain.Aggregates.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Application.Abstractions.Repositories;

public interface IMatchRepository : IRepositoryBase<EventId, Event>
{

}