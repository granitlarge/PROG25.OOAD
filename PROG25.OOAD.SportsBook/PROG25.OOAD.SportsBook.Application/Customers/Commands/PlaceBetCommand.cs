using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Application.Customers.Commands;

public record PlaceBetCommand
(
    CustomerId CustomerId,
    MarketId MarketId,
    OutcomeId OutcomeId,
    Money Stake
);