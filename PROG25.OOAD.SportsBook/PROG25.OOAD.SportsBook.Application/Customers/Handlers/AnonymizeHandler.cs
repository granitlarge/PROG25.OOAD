using PROG25.OOAD.SportsBook.Application.Customers.Commands;
using PROG25.OOAD.SportsBook.Application.Repositories;

namespace PROG25.OOAD.SportsBook.Application.Customers.Handlers;

public class AnonymizeHandler(IUnitOfWork unitOfWork)
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