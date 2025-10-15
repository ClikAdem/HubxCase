using MongoDB.Driver;
using Movie.Domain.Entities;
using Movie.Domain.Repositories;
using Movie.Infrastructure.Database;
using System.Linq.Expressions;

namespace Movie.Infrastructure.Repositories;


public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly IMongoCollection<T> _collection;

    protected BaseRepository(MongoDbContext context, string collectionName)
    {
        _collection = context.GetCollection<T>(collectionName);
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken ct = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        await _collection.InsertOneAsync(entity, cancellationToken: ct);
        return entity;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _collection.Find(_ => true).ToListAsync(ct);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
    }

    public virtual async Task<T?> UpdateAsync(T entity, CancellationToken ct = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        var result = await _collection.ReplaceOneAsync(
            x => x.Id == entity.Id,
            entity,
            cancellationToken: ct);

        return result.ModifiedCount > 0 ? entity : null;
    }

    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id, ct);
        return result.DeletedCount > 0;
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        var count = await _collection.CountDocumentsAsync(x => x.Id == id, cancellationToken: ct);
        return count > 0;
    }

    protected async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
    {
        return await _collection.Find(filter).ToListAsync(ct);
    }

    protected async Task<T?> FindOneAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
    {
        return await _collection.Find(filter).FirstOrDefaultAsync(ct);
    }
}