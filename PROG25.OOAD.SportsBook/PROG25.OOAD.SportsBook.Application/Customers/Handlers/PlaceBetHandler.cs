using PROG25.OOAD.SportsBook.Application.Customers.Commands;
using PROG25.OOAD.SportsBook.Application.Repositories;
using PROG25.OOAD.SportsBook.Domain.Services;

namespace PROG25.OOAD.SportsBook.Application.Customers.Handlers;

public class PlaceBetHandler
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
        var marketTasks = Task.WhenAll(command.MarketOutcomes.Select(mo => _unitOfWork.MarketRepository.GetByIdAsync(mo.MarketId)).ToList());

        await Task.WhenAll(customerTask, marketTasks);

        var customer = await customerTask ?? throw new InvalidOperationException($"Customer with ID {command.CustomerId} not found.");
        var markets = await marketTasks ?? throw new InvalidOperationException($"One or more markets not found for the provided market IDs: {string.Join(", ", command.MarketOutcomes.Select(mo => mo.MarketId))}.");

        if (markets.Any(market => market == null))
        {
            throw new InvalidOperationException("One or more markets not found for the provided market IDs.");
        }

        var marketAndOutcome = markets.Select(market =>
        {
            var (_, OutcomeId) = command.MarketOutcomes.First(mo => mo.MarketId == market!.Id);
            var outcome = market!.Outcomes.First(o => o.Id == OutcomeId);
            return (market, outcome);
        }).ToList();

        var bet = CustomerDomainService.PlaceBet(customer, marketAndOutcome, command.Stake);

        _unitOfWork.CustomerRepository.Update(customer);
        foreach (var market in markets)
        {
            _unitOfWork.MarketRepository.Update(market!);
        }
        _unitOfWork.BetRepository.Add(bet);

        await _unitOfWork.CommitAsync();
    }
}