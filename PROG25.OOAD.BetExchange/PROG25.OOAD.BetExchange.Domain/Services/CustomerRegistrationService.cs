using PROG25.OOAD.BetExchange.Domain.Aggregates;
using PROG25.OOAD.BetExchange.Domain.Interfaces.Policies.Customers.Registrations;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Domain.Services;

public class CustomerRegistrationService
{
    private readonly IAgeCustomerRegistrationPolicy _agePolicy;
    private readonly IDepositLimitsCustomerRegistrationPolicy _depositLimitsPolicy;

    public CustomerRegistrationService(IAgeCustomerRegistrationPolicy agePolicy, IDepositLimitsCustomerRegistrationPolicy depositLimitsPolicy)
    {
        _agePolicy = agePolicy;
        _depositLimitsPolicy = depositLimitsPolicy;
    }

    public Customer Register
    (
        Age age,
        PersonId personId,
        Name name,
        EmailAddress email,
        Currency currency,
        DepositLimits depositLimits
    )
    {
        if (!_agePolicy.IsValid(age))
        {
            throw new InvalidOperationException(_agePolicy.InvalidMessage);
        }

        if (!_depositLimitsPolicy.IsValid(depositLimits))
        {
            throw new InvalidOperationException(_depositLimitsPolicy.InvalidMessage);
        }

        return Customer.Create(personId, name, email, currency, depositLimits);
    }

}