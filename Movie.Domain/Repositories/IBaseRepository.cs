using Movie.Domain.Entities;

namespace Movie.Domain.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    // Creates a new entity
    Task<T> CreateAsync(T entity, CancellationToken ct = default);

    // Retrieves all entities
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);

    // Retrieves an entity by Id
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);

    // Updates an existing entity
    Task<T?> UpdateAsync(T entity, CancellationToken ct = default);

    // Deletes an entity by Id
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);

    // Checks if an entity exists by Id
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
}