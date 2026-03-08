using PROG25.OOAD.SportsBook.Domain.Aggregates;

namespace PROG25.OOAD.SportsBook.Domain.Interfaces.Policies.Customers.Modifications;

public interface ICustomerModificationPolicy<T> : IPolicy
{
    public bool IsValid(Customer customer, T value);
}