namespace PROG25.OOAD.SportsBook.Application.Abstractions;

public interface ICommandHandler<TCommand>
{
    Task Handle(TCommand command);
}