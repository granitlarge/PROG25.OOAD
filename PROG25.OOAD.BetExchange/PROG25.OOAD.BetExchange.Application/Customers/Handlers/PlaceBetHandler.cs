using PROG25.OOAD.BetExchange.Application.Abstractions;
using PROG25.OOAD.BetExchange.Application.Abstractions.Repositories;
using PROG25.OOAD.BetExchange.Application.Customers.Commands;
using PROG25.OOAD.BetExchange.Application.Services;
using PROG25.OOAD.BetExchange.Domain.Services;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Application.Customers.Handlers;

public class PlaceBetHandler : ICommandHandler<PlaceBetCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrencyService _currencyService;

    public PlaceBetHandler(IUnitOfWork unitOfWork, ICurrencyService currencyService)
    {
        _unitOfWork = unitOfWork;
        _currencyService = currencyService;
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

        var outcome = market.YesOutcome.Id == command.OutcomeId ? market.YesOutcome :
            market.NoOutcome.Id == command.OutcomeId ? market.NoOutcome :
            throw new ArgumentException($"Invalid outcome ID {command.OutcomeId} for market {market.Id}", nameof(command.OutcomeId));

        var stake = command.Stake;
        var bet = PlaceBetService.PlaceBet(customer, market, outcome, stake);

        _unitOfWork.CustomerRepository.Update(customer);
        _unitOfWork.MarketRepository.Update(market);
        _unitOfWork.BetRepository.Add(bet);

        await _unitOfWork.CommitAsync();
    }
}