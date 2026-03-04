namespace PROG25.OOAD.Domain.Repositories;

public interface IRepositoryBase<T_Id, T_Entity>
{
    Task<T_Entity> GetByIdAsync(T_Id id);
    Task AddAsync(T_Entity entity);
    Task UpdateAsync(T_Entity entity);
    Task DeleteAsync(T_Id id);
}