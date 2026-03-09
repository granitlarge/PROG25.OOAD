using PROG25.OOAD.SportsBook.Application.Abstractions;
using PROG25.OOAD.SportsBook.Application.Abstractions.Repositories;
using PROG25.OOAD.SportsBook.Application.Customers.Commands;
using PROG25.OOAD.SportsBook.Domain.Services;

namespace PROG25.OOAD.SportsBook.Application.Customers.Handlers;

public class PlaceBetHandler : ICommandHandler<PlaceBetCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public PlaceBetHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PlaceBetCommand command)
    {
        // What happens if the match state changes after we've loaded it but before the bet is inserted in the repository?
#warning The timestamp occurs before the bet is placed: ????
        // The timestamp occurs after the bet is placed: the bet is settled as usual.

        var customerTask = _unitOfWork.CustomerRepository.GetByIdAsync(command.CustomerId);
        var marketTask = _unitOfWork.MarketRepository.GetByIdAsync(command.MarketId);

        await Task.WhenAll(customerTask, marketTask);

        var customer = await customerTask ?? throw new InvalidOperationException($"Customer with ID {command.CustomerId} not found.");
        var market = await marketTask ?? throw new InvalidOperationException($"One or more markets not found for the provided market IDs: {command.MarketId}");

        var outcome = market.Outcomes.SingleOrDefault(o => o.Id == command.OutcomeId) ?? throw new InvalidOperationException($"Outcome {command.OutcomeId} is not part of market {command.MarketId}");
        var bet = PlaceBetService.PlaceBet(customer, market, outcome, command.Stake);

        _unitOfWork.CustomerRepository.Update(customer);
        _unitOfWork.MarketRepository.Update(market);
        _unitOfWork.BetRepository.Add(bet);

        await _unitOfWork.CommitAsync();
    }
}