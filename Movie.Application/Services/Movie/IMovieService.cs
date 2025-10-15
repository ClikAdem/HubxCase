using Movie.Application.Dtos.Movie;

namespace Movie.Application.Services.Movie;

public interface IMovieService
{
    Task<MovieDto> CreateMovieAsync(CreateMovieRequest request, CancellationToken ct = default);

    Task<IEnumerable<MovieDto>> GetAllMoviesAsync(CancellationToken ct = default);

    Task<MovieDto> GetMovieByIdAsync(Guid id, CancellationToken ct = default);

    Task<MovieDto> UpdateMovieAsync(UpdateMovieRequest request, CancellationToken ct = default);

    Task DeleteMovieAsync(Guid id, CancellationToken ct = default);
}