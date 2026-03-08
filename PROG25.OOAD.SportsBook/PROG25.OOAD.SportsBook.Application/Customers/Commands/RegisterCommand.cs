using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Application.Customers.Commands;

public record RegisterCommand
(
    PersonId PersonId,
    Age Age,
    Name Name,
    EmailAddress Email,
    Currency Currency,
    DepositLimits DepositLimits
);