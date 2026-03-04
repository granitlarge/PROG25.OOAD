using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Application.Customers.Commands;

public record RegisterCommand
(
    PersonId PersonId,
    Name Name,
    EmailAddress Email,
    Currency Currency
);