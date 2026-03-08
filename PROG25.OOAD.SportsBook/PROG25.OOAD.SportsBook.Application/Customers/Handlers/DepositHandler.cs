using PROG25.OOAD.SportsBook.Application.Abstractions;
using PROG25.OOAD.SportsBook.Application.Abstractions.Repositories;
using PROG25.OOAD.SportsBook.Application.Customers.Commands;

namespace PROG25.OOAD.SportsBook.Application.Customers.Handlers
{
    public class DepositHandler : ICommandHandler<DepositCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public DepositHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DepositCommand command)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(command.CustomerId) ?? throw new InvalidOperationException($"Customer with ID {command.CustomerId} not found.");
            customer.Deposit(command.Money, DateTimeOffset.UtcNow);
            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.CommitAsync();
        }

    }
}