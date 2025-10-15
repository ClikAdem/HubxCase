using Microsoft.AspNetCore.Mvc;
using Movie.Application.Dtos.Director;
using Movie.Application.Services.Director;

namespace Movie.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DirectorsController : ControllerBase
{
    private readonly IDirectorService _directorService;

    public DirectorsController(IDirectorService directorService)
    {
        _directorService = directorService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DirectorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<DirectorDto>>> GetAll(CancellationToken ct)
    {
        var directors = await _directorService.GetAllDirectorsAsync(ct);
        return Ok(directors);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DirectorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DirectorDto>> GetById(Guid id, CancellationToken ct)
    {
        var director = await _directorService.GetDirectorByIdAsync(id, ct);
        return Ok(director);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DirectorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DirectorDto>> Create([FromBody] CreateDirectorRequest request, CancellationToken ct)
    {
        var director = await _directorService.CreateDirectorAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = director.Id }, director);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(DirectorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DirectorDto>> Update(Guid id, [FromBody] UpdateDirectorRequest request, CancellationToken ct)
    {
        if (id != request.Id)
        {
            return BadRequest(new { Title = "Validation Failed", Status = 400, Detail = "ID mismatch" });
        }

        var director = await _directorService.UpdateDirectorAsync(request, ct);
        return Ok(director);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _directorService.DeleteDirectorAsync(id, ct);
        return NoContent();
    }
}