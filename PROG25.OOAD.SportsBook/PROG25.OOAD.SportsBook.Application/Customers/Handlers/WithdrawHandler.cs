using PROG25.OOAD.SportsBook.Application.Abstractions;
using PROG25.OOAD.SportsBook.Application.Abstractions.Repositories;
using PROG25.OOAD.SportsBook.Application.Customers.Commands;

namespace PROG25.OOAD.SportsBook.Application.Customers.Handlers;

public class WithdrawHandler : ICommandHandler<WithdrawCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public WithdrawHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(WithdrawCommand command)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(command.CustomerId) ?? throw new InvalidOperationException($"Customer with ID {command.CustomerId} not found.");
        customer.Withdraw(command.Money);
        _unitOfWork.CustomerRepository.Update(customer);
        await _unitOfWork.CommitAsync();
    }
}