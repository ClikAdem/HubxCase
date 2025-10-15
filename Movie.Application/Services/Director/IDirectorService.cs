using Movie.Application.Dtos.Director;

namespace Movie.Application.Services.Director;

public interface IDirectorService
{
    Task<DirectorDto> CreateDirectorAsync(CreateDirectorRequest request, CancellationToken ct = default);

    Task<IEnumerable<DirectorDto>> GetAllDirectorsAsync(CancellationToken ct = default);

    Task<DirectorDto> GetDirectorByIdAsync(Guid id, CancellationToken ct = default);

    Task<DirectorDto> UpdateDirectorAsync(UpdateDirectorRequest request, CancellationToken ct = default);

    Task DeleteDirectorAsync(Guid id, CancellationToken ct = default);
}