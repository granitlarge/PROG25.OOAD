using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Application.Customers.Commands;

public record PlaceBetCommand
(
    CustomerId CustomerId,
    ISet<(MarketId MarketId, OutcomeId OutcomeId)> MarketOutcomes,
    Money Stake
);