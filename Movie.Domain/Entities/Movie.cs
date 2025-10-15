using MongoDB.Bson.Serialization.Attributes;

namespace Movie.Domain.Entities;

[BsonCollection("movies")]
public class Movie : BaseEntity
{
    [BsonElement("title")]
    [BsonRequired]
    public string Title { get; set; } = default!;

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("releaseDate")]
    [BsonRequired]
    public DateTime ReleaseDate { get; set; }

    [BsonElement("genre")]
    [BsonRequired]
    public string Genre { get; set; } = default!;

    [BsonElement("rating")]
    [BsonRequired]
    public decimal Rating { get; set; }

    [BsonElement("imdbId")]
    [BsonRequired]
    public string ImdbId { get; set; } = default!;

    [BsonElement("directorId")]
    public Guid? DirectorId { get; set; }

    [BsonIgnore]
    public Director? Director { get; set; }
}