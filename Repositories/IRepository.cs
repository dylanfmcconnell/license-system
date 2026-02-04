public interface IRepository<T, TKey>
{
    Task<T?> GetEntityById(TKey id);
    Task<IEnumerable<T>> GetAllEntities();
    Task<bool> AddEntity(T entity);
    Task<bool> UpdateEntity(T entity);
    Task<bool> DeleteEntity(TKey id);
}