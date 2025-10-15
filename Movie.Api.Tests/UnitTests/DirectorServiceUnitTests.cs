using AutoMapper;
using FluentAssertions;
using Moq;
using Movie.Application.Dtos.Director;
using Movie.Application.Services.Director;
using Movie.Domain.Entities;
using Movie.Domain.Exceptions;
using Movie.Domain.Repositories.Director;
using Movie.Domain.Repositories.Movie;

namespace Movie.Api.Tests.UnitTests;

public class DirectorServiceUnitTests
{
    private readonly Mock<IDirectorRepository> _mockDirectorRepository;
    private readonly Mock<IMovieRepository> _mockMovieRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DirectorService _directorService;

    public DirectorServiceUnitTests()
    {
        _mockDirectorRepository = new Mock<IDirectorRepository>();
        _mockMovieRepository = new Mock<IMovieRepository>();
        _mockMapper = new Mock<IMapper>();
        _directorService = new DirectorService(_mockDirectorRepository.Object, _mockMovieRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateDirectorAsync_ValidDirector_ReturnsDirectorDto()
    {
        // Arrange
        var request = new CreateDirectorRequest
        {
            FirstName = "Christopher",
            LastName = "Nolan",
            BirthDate = new DateTime(1970, 7, 30),
            Bio = "British-American film director"
        };

        var director = new Director
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate,
            Bio = request.Bio
        };

        var directorDto = new DirectorDto
        {
            Id = director.Id,
            FirstName = director.FirstName,
            LastName = director.LastName,
            FullName = $"{director.FirstName} {director.LastName}",
            BirthDate = director.BirthDate,
            Bio = director.Bio
        };

        _mockMapper.Setup(x => x.Map<Director>(request)).Returns(director);
        _mockDirectorRepository.Setup(x => x.CreateAsync(It.IsAny<Director>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(director);
        _mockMapper.Setup(x => x.Map<DirectorDto>(director)).Returns(directorDto);

        // Act
        var result = await _directorService.CreateDirectorAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be(request.FirstName);
        result.LastName.Should().Be(request.LastName);
    }

    [Fact]
    public async Task GetAllDirectorsAsync_ReturnsAllDirectors()
    {
        // Arrange
        var directors = new List<Director>
        {
            new() { Id = Guid.NewGuid(), FirstName = "Christopher", LastName = "Nolan", BirthDate = new DateTime(1970, 7, 30) },
            new() { Id = Guid.NewGuid(), FirstName = "Steven", LastName = "Spielberg", BirthDate = new DateTime(1946, 12, 18) }
        };

        var directorDtos = new List<DirectorDto>
        {
            new() { Id = directors[0].Id, FirstName = "Christopher", LastName = "Nolan", FullName = "Christopher Nolan" },
            new() { Id = directors[1].Id, FirstName = "Steven", LastName = "Spielberg", FullName = "Steven Spielberg" }
        };

        _mockDirectorRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(directors);
        _mockMapper.Setup(x => x.Map<IEnumerable<DirectorDto>>(directors))
            .Returns(directorDtos);

        // Act
        var result = await _directorService.GetAllDirectorsAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetDirectorByIdAsync_ExistingId_ReturnsDirector()
    {
        // Arrange
        var directorId = Guid.NewGuid();
        var director = new Director
        {
            Id = directorId,
            FirstName = "Christopher",
            LastName = "Nolan",
            BirthDate = new DateTime(1970, 7, 30)
        };

        var directorDto = new DirectorDto
        {
            Id = directorId,
            FirstName = director.FirstName,
            LastName = director.LastName,
            FullName = "Christopher Nolan"
        };

        _mockDirectorRepository.Setup(x => x.GetByIdAsync(directorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(director);
        _mockMapper.Setup(x => x.Map<DirectorDto>(director)).Returns(directorDto);

        // Act
        var result = await _directorService.GetDirectorByIdAsync(directorId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(directorId);
    }

    [Fact]
    public async Task GetDirectorByIdAsync_NonExistingId_ThrowsBusinessException()
    {
        // Arrange
        var directorId = Guid.NewGuid();
        _mockDirectorRepository.Setup(x => x.GetByIdAsync(directorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Director?)null);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessException>(() => _directorService.GetDirectorByIdAsync(directorId));
    }

    [Fact]
    public async Task DeleteDirectorAsync_DirectorWithMovies_ThrowsBusinessException()
    {
        // Arrange
        var directorId = Guid.NewGuid();
        var movies = new List<Domain.Entities.Movie>
        {
            new() { Id = Guid.NewGuid(), DirectorId = directorId, Title = "Test Movie", ImdbId = "tt1234567", Genre = "Action", Rating = 8.0m, ReleaseDate = DateTime.UtcNow }
        };

        _mockDirectorRepository.Setup(x => x.ExistsAsync(directorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockMovieRepository.Setup(x => x.GetByDirectorIdAsync(directorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(movies);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() => _directorService.DeleteDirectorAsync(directorId));
        exception.ExceptionMessage.Should().Contain("Cannot delete director with associated movies");
    }

    [Fact]
    public async Task DeleteDirectorAsync_DirectorWithoutMovies_DoesNotThrow()
    {
        // Arrange
        var directorId = Guid.NewGuid();
        _mockDirectorRepository.Setup(x => x.ExistsAsync(directorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockMovieRepository.Setup(x => x.GetByDirectorIdAsync(directorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Entities.Movie>());
        _mockDirectorRepository.Setup(x => x.DeleteAsync(directorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _directorService.DeleteDirectorAsync(directorId);

        // Assert
        await act.Should().NotThrowAsync();
    }
}