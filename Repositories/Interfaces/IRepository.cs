/// <summary>
/// Generic repository interface providing standard CRUD operations for entities.
/// </summary>
/// <typeparam name="T">The entity type managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
public interface IRepository<T, TKey>
{
    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    Task<T?> GetEntityById(TKey id);

    /// <summary>
    /// Retrieves all entities of this type.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    Task<IEnumerable<T>> GetAllEntities();

    /// <summary>
    /// Adds a new entity to the data store.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity with its generated identifier, or <c>null</c> if the operation failed.</returns>
    Task<T?> AddEntity(T entity);

    /// <summary>
    /// Updates an existing entity in the data store.
    /// </summary>
    /// <param name="entity">The entity with updated values.</param>
    /// <returns><c>true</c> if the entity was successfully updated; otherwise, <c>false</c>.</returns>
    Task<bool> UpdateEntity(T entity);

    /// <summary>
    /// Deletes an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns><c>true</c> if the entity was successfully deleted; otherwise, <c>false</c>.</returns>
    Task<bool> DeleteEntity(TKey id);
}
