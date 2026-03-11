using PROG25.OOAD.BetExchange.Domain.Aggregates;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Bets;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Domain.Services;

public class PlaceBetService
{
    // The customer's balance is in a certain currency.
    // The bet is in the currency of the customer.
    // The way we count exposure is in EUR, so we need to convert the stake from the customers currency to EUR.
    public static Bet PlaceBet(Customer customer, EventMetricMarket market, Outcome outcome, Money stake)
    {
        var bet = market.PlaceBet(customer.Id, stake, outcome);
        customer.PlaceBet(stake);
        return bet;
    }
}