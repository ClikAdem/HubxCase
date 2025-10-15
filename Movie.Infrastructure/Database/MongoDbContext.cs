using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Movie.Domain.Entities;

namespace Movie.Infrastructure.Database;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private static bool _isConfigured = false;
    private static readonly object _lock = new object();

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        // Configure MongoDB GUID serialization (only once)
        ConfigureMongoDB();

        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    private static void ConfigureMongoDB()
    {
        if (_isConfigured)
            return;

        lock (_lock)
        {
            if (_isConfigured)
                return;

            // Configure GUID serialization to use standard representation
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            _isConfigured = true;
        }
    }

    public IMongoCollection<T> GetCollection<T>(string name) where T : BaseEntity
    {
        return _database.GetCollection<T>(name);
    }

    public IMongoCollection<Domain.Entities.Movie> Movies => GetCollection<Domain.Entities.Movie>("movies");
    public IMongoCollection<Director> Directors => GetCollection<Director>("directors");
}