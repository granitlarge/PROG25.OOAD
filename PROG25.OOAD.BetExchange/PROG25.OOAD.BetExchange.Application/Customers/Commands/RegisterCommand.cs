using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Application.Customers.Commands;

public record RegisterCommand
(
    PersonId PersonId,
    Age Age,
    Name Name,
    EmailAddress Email,
    Currency Currency,
    DepositLimits DepositLimits
);