using AutoMapper;
using Movie.Application.Dtos.Director;
using Movie.Domain.Exceptions;
using Movie.Domain.Repositories.Director;
using Movie.Domain.Repositories.Movie;

namespace Movie.Application.Services.Director;

public class DirectorService : IDirectorService
{
    private readonly IDirectorRepository _directorRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;

    public DirectorService(IDirectorRepository directorRepository, IMovieRepository movieRepository, IMapper mapper)
    {
        _directorRepository = directorRepository;
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public async Task<DirectorDto> CreateDirectorAsync(CreateDirectorRequest request, CancellationToken ct = default)
    {
        var director = _mapper.Map<Domain.Entities.Director>(request);
        var createdDirector = await _directorRepository.CreateAsync(director, ct);
        return _mapper.Map<DirectorDto>(createdDirector);
    }

    public async Task<IEnumerable<DirectorDto>> GetAllDirectorsAsync(CancellationToken ct = default)
    {
        var directors = await _directorRepository.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<DirectorDto>>(directors);
    }

    public async Task<DirectorDto> GetDirectorByIdAsync(Guid id, CancellationToken ct = default)
    {
        var director = await _directorRepository.GetByIdAsync(id, ct);
        if (director == null)
        {
            throw new BusinessException("Director not found");
        }

        return _mapper.Map<DirectorDto>(director);
    }

    public async Task<DirectorDto> UpdateDirectorAsync(UpdateDirectorRequest request, CancellationToken ct = default)
    {
        var existingDirector = await _directorRepository.GetByIdAsync(request.Id, ct);
        if (existingDirector == null)
        {
            throw new BusinessException("Director not found");
        }

        var director = _mapper.Map<Domain.Entities.Director>(request);
        director.CreatedAt = existingDirector.CreatedAt;
        director.UpdatedAt = DateTime.UtcNow;

        var updatedDirector = await _directorRepository.UpdateAsync(director, ct);
        if (updatedDirector == null)
        {
            throw new BusinessException("Failed to update director");
        }

        return _mapper.Map<DirectorDto>(updatedDirector);
    }

    public async Task DeleteDirectorAsync(Guid id, CancellationToken ct = default)
    {
        var exists = await _directorRepository.ExistsAsync(id, ct);
        if (!exists)
        {
            throw new BusinessException("Director not found");
        }

        var movies = await _movieRepository.GetByDirectorIdAsync(id, ct);
        if (movies.Any())
        {
            throw new BusinessException(
                "Cannot delete director with associated movies. Please remove or reassign the movies first.");
        }

        var result = await _directorRepository.DeleteAsync(id, ct);
        if (!result)
        {
            throw new BusinessException("Failed to delete director");
        }
    }

}