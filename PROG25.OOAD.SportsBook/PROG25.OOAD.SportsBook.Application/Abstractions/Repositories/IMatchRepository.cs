using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Application.Abstractions.Repositories;

public interface IMatchRepository : IRepositoryBase<EventId, Event>
{

}