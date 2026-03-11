using PROG25.OOAD.BetExchange.Application.Abstractions;
using PROG25.OOAD.BetExchange.Application.Abstractions.Repositories;
using PROG25.OOAD.BetExchange.Application.Customers.Commands;

namespace PROG25.OOAD.BetExchange.Application.Customers.Handlers;

public class AnonymizeHandler(IUnitOfWork unitOfWork) : ICommandHandler<AnonymizeCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(AnonymizeCommand command)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(command.CustomerId) ?? throw new InvalidOperationException($"Customer with ID {command.CustomerId.Value} not found.");
        customer.Anonymize();
        _unitOfWork.CustomerRepository.Update(customer);
        await _unitOfWork.CommitAsync();
    }
}