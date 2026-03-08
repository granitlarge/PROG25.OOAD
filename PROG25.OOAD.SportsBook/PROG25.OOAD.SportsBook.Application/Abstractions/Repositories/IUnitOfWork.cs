namespace PROG25.OOAD.SportsBook.Application.Abstractions.Repositories;

public interface IUnitOfWork
{
    ICustomerRepository CustomerRepository { get; }
    IMarketRepository MarketRepository { get; }
    IMatchRepository MatchRepository { get; }
    IBetRepository BetRepository { get; }
    Task CommitAsync();
}