using PROG25.OOAD.BetExchange.Domain.Aggregates.Bets;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Oddss;

namespace PROG25.OOAD.BetExchange.Application.Customers.Commands;

public record PlaceBetCommand
(
    CustomerId CustomerId,
    MarketId MarketId,
    OutcomeId OutcomeId,
    Money Stake,
    Side Side,
    Odds Odds
);