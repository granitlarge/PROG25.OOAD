using PROG25.OOAD.Domain.Aggregates;
using PROG25.OOAD.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.Domain.Entities.Outcome;
using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Services;

public class BetService
{
    public Bet PlaceBet(Customer customer,
        Market market,
        Outcome outcome,
        Money amount)
    {
        market.EnsureBetCanBePlaced(outcome.Id);
        customer.PlaceBet(amount);
        return Bet.Place(market.Id, customer.Id, outcome.Id, amount, outcome.Odds);
    }
}