using MongoDB.Bson.Serialization.Attributes;

namespace Movie.Domain.Entities;

[BsonCollection("directors")]
public class Director : BaseEntity
{
    [BsonElement("firstName")]
    [BsonRequired]
    public string FirstName { get; set; } = default!;

    [BsonElement("lastName")]
    [BsonRequired]
    public string LastName { get; set; } = default!;

    [BsonElement("birthDate")]
    [BsonRequired]
    public DateTime BirthDate { get; set; }

    [BsonElement("bio")]
    public string? Bio { get; set; }

    [BsonIgnore]
    public string FullName => $"{FirstName} {LastName}";
}


// Attribute to name MongoDB collection
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    public string CollectionName { get; }

    public BsonCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}