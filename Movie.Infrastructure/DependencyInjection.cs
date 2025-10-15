using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movie.Domain.Repositories.Director;
using Movie.Domain.Repositories.Movie;
using Movie.Infrastructure.Caching;
using Movie.Infrastructure.Database;
using Movie.Infrastructure.Repositories;

namespace Movie.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure MongoDB
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.AddSingleton<MongoDbContext>();

        // Register Repositories
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IDirectorRepository, DirectorRepository>();

        // Configure Redis
        var redisSettings = configuration.GetSection("RedisSettings").Get<RedisSettings>();
        if (redisSettings != null && !string.IsNullOrEmpty(redisSettings.ConnectionString))
        {
            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisSettings.ConnectionString;
                options.InstanceName = "MovieAPI_";
            });
            services.AddSingleton<ICacheService, RedisCacheService>();
        }
        else
        {
            // If Redis is not configured, use in-memory cache as fallback
            services.AddDistributedMemoryCache();
            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
            services.AddSingleton<ICacheService, RedisCacheService>();
        }

        return services;
    }
}