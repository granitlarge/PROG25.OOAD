using PROG25.OOAD.BetExchange.Application.Abstractions;
using PROG25.OOAD.BetExchange.Application.Abstractions.Repositories;
using PROG25.OOAD.BetExchange.Application.Customers.Commands;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Application.Customers.Handlers;

public class SelfExcludeHandler : ICommandHandler<SelfExcludeCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public SelfExcludeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SelfExcludeCommand command)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(command.CustomerId) ?? throw new InvalidOperationException($"Customer with ID {command.CustomerId} not found.");
        customer.ResponsibleGambling.SelfExclude(new SelfExclusion(command.EndDate));
        _unitOfWork.CustomerRepository.Update(customer);
        await _unitOfWork.CommitAsync();
    }
}