using PROG25.OOAD.SportsBook.Domain.Aggregates;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Bets;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Services;

public class PlaceBetService
{
    public static Bet PlaceBet(Customer customer, EventMetricMarket market, Outcome outcome, Money stake)
    {
        var bet = market.PlaceBet(customer.Id, stake, outcome);
        customer.PlaceBet(stake);
        return bet;
    }
}