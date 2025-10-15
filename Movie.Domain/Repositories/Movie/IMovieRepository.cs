namespace Movie.Domain.Repositories.Movie;

public interface IMovieRepository : IBaseRepository<Entities.Movie>
{
    Task<IEnumerable<Entities.Movie>> GetByDirectorIdAsync(Guid directorId, CancellationToken ct = default);

    Task<IEnumerable<Entities.Movie>> GetByGenreAsync(string genre, CancellationToken ct = default);

    Task<Entities.Movie?> GetByImdbIdAsync(string imdbId, CancellationToken ct = default);
}