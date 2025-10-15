using Microsoft.Extensions.DependencyInjection;
using Movie.Application.Services.Director;
using Movie.Application.Services.Movie;
using System.Reflection;
using FluentValidation;

namespace Movie.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IDirectorService, DirectorService>();

        return services;
    }
}