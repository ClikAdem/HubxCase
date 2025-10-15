using Microsoft.AspNetCore.Mvc;
using Movie.Application.Dtos.Movie;
using Movie.Application.Services.Movie;

namespace Movie.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MovieDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetAll(CancellationToken ct)
    {
        var movies = await _movieService.GetAllMoviesAsync(ct);
        return Ok(movies);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MovieDto>> GetById(Guid id, CancellationToken ct)
    {
        var movie = await _movieService.GetMovieByIdAsync(id, ct);
        return Ok(movie);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MovieDto>> Create([FromBody] CreateMovieRequest request, CancellationToken ct)
    {
        var movie = await _movieService.CreateMovieAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MovieDto>> Update(Guid id, [FromBody] UpdateMovieRequest request, CancellationToken ct)
    {
        if (id != request.Id)
        {
            return BadRequest(new { Title = "Validation Failed", Status = 400, Detail = "ID mismatch" });
        }

        var movie = await _movieService.UpdateMovieAsync(request, ct);
        return Ok(movie);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _movieService.DeleteMovieAsync(id, ct);
        return NoContent();
    }
}