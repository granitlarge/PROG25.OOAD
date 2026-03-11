namespace PROG25.OOAD.BetExchange.Application.Abstractions;

public interface ICommandHandler<TCommand>
{
    Task Handle(TCommand command);
}