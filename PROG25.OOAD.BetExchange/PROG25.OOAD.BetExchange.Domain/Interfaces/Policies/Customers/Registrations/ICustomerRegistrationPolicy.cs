namespace PROG25.OOAD.BetExchange.Domain.Interfaces.Policies.Customers.Registrations;

public interface ICustomerRegistrationPolicy<T> : IPolicy
{
    public bool IsValid(T value);
}
