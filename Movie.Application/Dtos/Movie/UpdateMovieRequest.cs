namespace Movie.Application.Dtos.Movie;

public class UpdateMovieRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; } = default!;
    public decimal Rating { get; set; }
    public string ImdbId { get; set; } = default!;
    public Guid? DirectorId { get; set; }
}