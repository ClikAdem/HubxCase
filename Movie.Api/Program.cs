using FluentValidation;
using FluentValidation.AspNetCore;
using Movie.Api.Filters;
using Movie.Api.Middleware;
using Movie.Application;
using Movie.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(options =>
{
    // Add custom validation filter to handle validation errors automatically
    options.Filters.Add<CustomValidationFilter>();
});

// Add FluentValidation with auto validation
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

// Register validators from Application layer
builder.Services.AddValidatorsFromAssemblyContaining<Movie.Application.Services.Movie.MovieService>();
builder.Services.AddValidatorsFromAssemblyContaining<Movie.Application.Services.Director.DirectorService>();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Movie API",
        Version = "v1",
        Description = "A RESTful API For Backend DEveloper Case",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "HubX Backend Developer Case",
            Email = "ademcelik6868@gmail.com"
        }
    });

    // Enable XML comments for better documentation ? (TODO:kullanmazsam silerim)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Exception Handling Middleware
app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();