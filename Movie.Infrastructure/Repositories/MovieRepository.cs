using Movie.Domain.Repositories.Movie;
using Movie.Infrastructure.Database;

namespace Movie.Infrastructure.Repositories;

public class MovieRepository : BaseRepository<Domain.Entities.Movie>, IMovieRepository
{
    public MovieRepository(MongoDbContext context) : base(context, "movies")
    {
    }

    public async Task<IEnumerable<Domain.Entities.Movie>> GetByDirectorIdAsync(Guid directorId, CancellationToken ct = default)
    {
        return await FindAsync(m => m.DirectorId == directorId, ct);
    }

    public async Task<IEnumerable<Domain.Entities.Movie>> GetByGenreAsync(string genre, CancellationToken ct = default)
    {
        return await FindAsync(m => m.Genre.Equals(genre, StringComparison.CurrentCultureIgnoreCase), ct);
    }

    public async Task<Domain.Entities.Movie?> GetByImdbIdAsync(string imdbId, CancellationToken ct = default)
    {
        return await FindOneAsync(m => m.ImdbId == imdbId, ct);
    }
}