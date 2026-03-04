using PROG25.OOAD.SportsBook.Domain.Aggregates;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Application.Repositories;

public interface ICustomerRepository : IRepositoryBase<CustomerId, Customer>
{
    Task<bool> ExistsByPersonIdAsync(PersonId personId);
}