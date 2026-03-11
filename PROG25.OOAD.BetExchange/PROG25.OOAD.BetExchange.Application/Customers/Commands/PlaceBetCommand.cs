using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Application.Customers.Commands;

public record PlaceBetCommand
(
    CustomerId CustomerId,
    MarketId MarketId,
    OutcomeId OutcomeId,
    Money Stake
);