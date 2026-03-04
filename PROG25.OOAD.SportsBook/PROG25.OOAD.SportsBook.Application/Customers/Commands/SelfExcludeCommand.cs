using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Application.Customers.Commands;

public sealed record SelfExcludeCommand
(
    CustomerId CustomerId,
    DateTimeOffset EndDate
);