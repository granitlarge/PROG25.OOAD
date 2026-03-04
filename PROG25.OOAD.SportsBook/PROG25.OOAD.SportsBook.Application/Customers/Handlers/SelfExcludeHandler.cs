using PROG25.OOAD.SportsBook.Application.Customers.Commands;
using PROG25.OOAD.SportsBook.Application.Repositories;

namespace PROG25.OOAD.SportsBook.Application.Customers.Handlers;

public class SelfExcludeHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public SelfExcludeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SelfExcludeCommand command)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(command.CustomerId) ?? throw new InvalidOperationException($"Customer with ID {command.CustomerId} not found.");
        customer.SelfExclude(command.EndDate);
        _unitOfWork.CustomerRepository.Update(customer);
        await _unitOfWork.CommitAsync();
    }
}