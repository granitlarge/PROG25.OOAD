using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Application.Customers.Commands;

public sealed record SelfExcludeCommand
(
    CustomerId CustomerId,
    DateTimeOffset EndDate
);