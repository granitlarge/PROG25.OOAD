using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Application.Customers.Commands;

public record DepositCommand
(
    CustomerId CustomerId,
    Money Money
);