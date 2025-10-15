namespace Movie.Application.Dtos.Director;

public class CreateDirectorRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime BirthDate { get; set; }
    public string? Bio { get; set; }
}