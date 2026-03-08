namespace PROG25.OOAD.SportsBook.Domain.Interfaces.Policies.Customers.Registrations;

public interface ICustomerRegistrationPolicy<T> : IPolicy
{
    public bool IsValid(T value);
}
