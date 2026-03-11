using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Application.Customers.Commands;

public record WithdrawCommand
(
    CustomerId CustomerId,
    Money Money
);