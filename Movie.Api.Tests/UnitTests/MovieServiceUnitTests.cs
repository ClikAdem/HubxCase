using AutoMapper;
using FluentAssertions;
using Moq;
using Movie.Application.Dtos.Movie;
using Movie.Application.Services;
using Movie.Application.Services.Movie;
using Movie.Domain.Exceptions;
using Movie.Domain.Repositories;
using Movie.Domain.Repositories.Director;
using Movie.Domain.Repositories.Movie;
using Xunit;

namespace Movie.Api.Tests.UnitTests;

public class MovieServiceUnitTests
{
    private readonly Mock<IMovieRepository> _mockMovieRepository;
    private readonly Mock<IDirectorRepository> _mockDirectorRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MovieService _movieService;

    public MovieServiceUnitTests()
    {
        _mockMovieRepository = new Mock<IMovieRepository>();
        _mockDirectorRepository = new Mock<IDirectorRepository>();
        _mockMapper = new Mock<IMapper>();
        _movieService = new MovieService(_mockMovieRepository.Object, _mockDirectorRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateMovieAsync_ValidMovie_ReturnsMovieDto()
    {
        // Arrange
        var request = new CreateMovieRequest
        {
            Title = "Test Movie",
            Description = "Test Description",
            ReleaseDate = DateTime.UtcNow,
            Genre = "Action",
            Rating = 8.5m,
            ImdbId = "tt1234567",
            DirectorId = null
        };

        var movie = new Domain.Entities.Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            ReleaseDate = request.ReleaseDate,
            Genre = request.Genre,
            Rating = request.Rating,
            ImdbId = request.ImdbId
        };

        var movieDto = new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseDate = movie.ReleaseDate,
            Genre = movie.Genre,
            Rating = movie.Rating,
            ImdbId = movie.ImdbId
        };

        _mockMovieRepository.Setup(x => x.GetByImdbIdAsync(request.ImdbId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Movie?)null);
        _mockMapper.Setup(x => x.Map<Domain.Entities.Movie>(request)).Returns(movie);
        _mockMovieRepository.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.Movie>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(movie);
        _mockMapper.Setup(x => x.Map<MovieDto>(movie)).Returns(movieDto);

        // Act
        var result = await _movieService.CreateMovieAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
        result.ImdbId.Should().Be(request.ImdbId);
    }

    [Fact]
    public async Task CreateMovieAsync_DuplicateImdbId_ThrowsBusinessException()
    {
        // Arrange
        var request = new CreateMovieRequest
        {
            Title = "Test Movie",
            ImdbId = "tt1234567",
            ReleaseDate = DateTime.UtcNow,
            Genre = "Action",
            Rating = 8.5m
        };

        var existingMovie = new Domain.Entities.Movie { Id = Guid.NewGuid(), ImdbId = request.ImdbId };

        _mockMovieRepository.Setup(x => x.GetByImdbIdAsync(request.ImdbId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingMovie);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessException>(() => _movieService.CreateMovieAsync(request));
    }

    [Fact]
    public async Task GetAllMoviesAsync_ReturnsAllMovies()
    {
        // Arrange
        var movies = new List<Domain.Entities.Movie>
        {
            new() { Id = Guid.NewGuid(), Title = "Movie 1", ImdbId = "tt1111111", Genre = "Action", Rating = 8.0m, ReleaseDate = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Movie 2", ImdbId = "tt2222222", Genre = "Drama", Rating = 7.5m, ReleaseDate = DateTime.UtcNow }
        };

        var movieDtos = new List<MovieDto>
        {
            new() { Id = movies[0].Id, Title = "Movie 1", ImdbId = "tt1111111", Genre = "Action", Rating = 8.0m },
            new() { Id = movies[1].Id, Title = "Movie 2", ImdbId = "tt2222222", Genre = "Drama", Rating = 7.5m }
        };

        _mockMovieRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(movies);
        _mockMapper.Setup(x => x.Map<IEnumerable<MovieDto>>(movies))
            .Returns(movieDtos);

        // Act
        var result = await _movieService.GetAllMoviesAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetMovieByIdAsync_ExistingId_ReturnsMovie()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        var movie = new Domain.Entities.Movie
        {
            Id = movieId,
            Title = "Test Movie",
            ImdbId = "tt1234567",
            Genre = "Action",
            Rating = 8.5m,
            ReleaseDate = DateTime.UtcNow
        };

        var movieDto = new MovieDto
        {
            Id = movieId,
            Title = movie.Title,
            ImdbId = movie.ImdbId,
            Genre = movie.Genre,
            Rating = movie.Rating
        };

        _mockMovieRepository.Setup(x => x.GetByIdAsync(movieId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(movie);
        _mockMapper.Setup(x => x.Map<MovieDto>(movie)).Returns(movieDto);

        // Act
        var result = await _movieService.GetMovieByIdAsync(movieId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(movieId);
    }

    [Fact]
    public async Task GetMovieByIdAsync_NonExistingId_ThrowsBusinessException()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        _mockMovieRepository.Setup(x => x.GetByIdAsync(movieId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Movie?)null);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessException>(() => _movieService.GetMovieByIdAsync(movieId));
    }

    [Fact]
    public async Task DeleteMovieAsync_ExistingMovie_DoesNotThrow()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        _mockMovieRepository.Setup(x => x.ExistsAsync(movieId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockMovieRepository.Setup(x => x.DeleteAsync(movieId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _movieService.DeleteMovieAsync(movieId);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task DeleteMovieAsync_NonExistingMovie_ThrowsBusinessException()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        _mockMovieRepository.Setup(x => x.ExistsAsync(movieId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessException>(() => _movieService.DeleteMovieAsync(movieId));
    }
}
