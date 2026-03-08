using PROG25.OOAD.SportsBook.Application.Abstractions;
using PROG25.OOAD.SportsBook.Application.Abstractions.Repositories;
using PROG25.OOAD.SportsBook.Application.Customers.Commands;
using PROG25.OOAD.SportsBook.Domain.Aggregates;
using PROG25.OOAD.SportsBook.Domain.Services;

namespace PROG25.OOAD.SportsBook.Application.Customers.Handlers;

public class RegisterHandler : ICommandHandler<RegisterCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CustomerRegistrationService _customerRegistrationService;

    public RegisterHandler(IUnitOfWork unitOfWork, CustomerRegistrationService customerRegistrationService)
    {
        _unitOfWork = unitOfWork;
        _customerRegistrationService = customerRegistrationService;
    }

    public async Task Handle(RegisterCommand command)
    {
        // What happens if a customer with the same person id is registered while this command is being processed?
        // Transactional issue, how do we solve it?
        // Lock the entire customer table -> only 1 customer can be registered at a time, not good for performance.
        // (*) Add a unique constraint in the database -> business rule is enforced at the database level, which isn't good, because we want to enforce business rules in the domain layer, but is the least worst option.

        var customerWithPersonIdExists = await _unitOfWork.CustomerRepository.ExistsByPersonIdAsync(command.PersonId);
        var customer = _customerRegistrationService.Register
        (
            command.Age,
            command.PersonId,
            command.Name,
            command.Email,
            command.Currency,
            command.DepositLimits
        );
        _unitOfWork.CustomerRepository.Add(customer);
        await _unitOfWork.CommitAsync();
    }
}