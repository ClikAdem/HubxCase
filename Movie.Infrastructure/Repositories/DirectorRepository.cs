using MongoDB.Driver;
using Movie.Domain.Entities;
using Movie.Domain.Repositories.Director;
using Movie.Infrastructure.Database;

namespace Movie.Infrastructure.Repositories;

public class DirectorRepository : BaseRepository<Director>, IDirectorRepository
{
    public DirectorRepository(MongoDbContext context) : base(context, "directors")
    {
    }
}