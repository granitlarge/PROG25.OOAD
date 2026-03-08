namespace PROG25.OOAD.SportsBook.Application.Abstractions.Repositories;

public interface IRepositoryBase<T_Id, T_Entity>
{
    Task<T_Entity?> GetByIdAsync(T_Id id);
    void Add(T_Entity entity);
    void Update(T_Entity entity);
    void Delete(T_Id id);
}