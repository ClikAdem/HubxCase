using AutoMapper;
using Movie.Application.Dtos.Movie;
using Movie.Domain.Exceptions;
using Movie.Domain.Repositories.Director;
using Movie.Domain.Repositories.Movie;

namespace Movie.Application.Services.Movie;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IDirectorRepository _directorRepository;
    private readonly IMapper _mapper;

    public MovieService(IMovieRepository movieRepository, IDirectorRepository directorRepository, IMapper mapper)
    {
        _movieRepository = movieRepository;
        _directorRepository = directorRepository;
        _mapper = mapper;
    }

    public async Task<MovieDto> CreateMovieAsync(CreateMovieRequest request, CancellationToken ct = default)
    {
        if (request.DirectorId.HasValue)
        {
            var directorExists = await _directorRepository.ExistsAsync(request.DirectorId.Value, ct);
            if (!directorExists)
            {
                throw new BusinessException("Director not found");
            }
        }

        var existingMovie = await _movieRepository.GetByImdbIdAsync(request.ImdbId, ct);
        if (existingMovie != null)
        {
            throw new BusinessException("Movie with this IMDb ID already exists");
        }

        var movie = _mapper.Map<Domain.Entities.Movie>(request);
        var createdMovie = await _movieRepository.CreateAsync(movie, ct);

        if (createdMovie.DirectorId.HasValue)
        {
            createdMovie.Director = await _directorRepository.GetByIdAsync(createdMovie.DirectorId.Value, ct);
        }

        return _mapper.Map<MovieDto>(createdMovie);
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync(CancellationToken ct = default)
    {
        var movies = await _movieRepository.GetAllAsync(ct);

        foreach (var movie in movies)
        {
            if (movie.DirectorId.HasValue)
            {
                movie.Director = await _directorRepository.GetByIdAsync(movie.DirectorId.Value, ct);
            }
        }

        return _mapper.Map<IEnumerable<MovieDto>>(movies);
    }

    public async Task<MovieDto> GetMovieByIdAsync(Guid id, CancellationToken ct = default)
    {
        var movie = await _movieRepository.GetByIdAsync(id, ct);
        if (movie == null)
        {
            throw new BusinessException("Movie not found");
        }

        if (movie.DirectorId.HasValue)
        {
            movie.Director = await _directorRepository.GetByIdAsync(movie.DirectorId.Value, ct);
        }

        return _mapper.Map<MovieDto>(movie);
    }

    public async Task<MovieDto> UpdateMovieAsync(UpdateMovieRequest request, CancellationToken ct = default)
    {
        var existingMovie = await _movieRepository.GetByIdAsync(request.Id, ct);
        if (existingMovie == null)
        {
            throw new BusinessException("Movie not found");
        }

        if (request.DirectorId.HasValue)
        {
            var directorExists = await _directorRepository.ExistsAsync(request.DirectorId.Value, ct);
            if (!directorExists)
            {
                throw new BusinessException("Director not found");
            }
        }

        // Check if IMDb ID is being changed and if it conflicts with another movie
        if (request.ImdbId != existingMovie.ImdbId)
        {
            var movieWithSameImdbId = await _movieRepository.GetByImdbIdAsync(request.ImdbId, ct);
            if (movieWithSameImdbId != null && movieWithSameImdbId.Id != request.Id)
            {
                throw new BusinessException("Another movie with this IMDb ID already exists");
            }
        }

        var movie = _mapper.Map<Domain.Entities.Movie>(request);
        movie.CreatedAt = existingMovie.CreatedAt;
        movie.UpdatedAt = DateTime.UtcNow;

        var updatedMovie = await _movieRepository.UpdateAsync(movie, ct);
        if (updatedMovie == null)
        {
            throw new BusinessException("Failed to update movie");
        }

        if (updatedMovie.DirectorId.HasValue)
        {
            updatedMovie.Director = await _directorRepository.GetByIdAsync(updatedMovie.DirectorId.Value, ct);
        }

        return _mapper.Map<MovieDto>(updatedMovie);
    }

    public async Task DeleteMovieAsync(Guid id, CancellationToken ct = default)
    {
        var exists = await _movieRepository.ExistsAsync(id, ct);
        if (!exists)
        {
            throw new BusinessException("Movie not found");
        }

        var result = await _movieRepository.DeleteAsync(id, ct);
        if (!result)
        {
            throw new BusinessException("Failed to delete movie");
        }
    }
}