using PROG25.OOAD.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.Aggregates;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Bets;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Services;

public class CustomerDomainService
{
    public static Bet PlaceBet(Customer customer, List<(EventMetricMarket Market, Outcome Outcome)> marketOutcomes, Money stake)
    {
        var betLegs = marketOutcomes.Select(mo => mo.Market.PlaceBetLeg(mo.Outcome)).ToList();
        var bet = Bet.Create(customer.Id, stake, betLegs);
        customer.Withdraw(stake, TransactionReason.BetPlacement);
        return bet;
    }
}