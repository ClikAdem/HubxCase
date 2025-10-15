namespace Movie.Infrastructure.Caching;

public class RedisSettings
{
    public string ConnectionString { get; set; } = default!;
    public int DefaultExpirationMinutes { get; set; } = 60;
}