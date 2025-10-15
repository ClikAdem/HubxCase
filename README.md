# Movie API - HubX Backend Developer Case

A RESTful API for managing movies and directors, built with .NET 9 following Clean Architecture principles.

## Project Overview

This project demonstrates a well-structured backend solution using modern technologies and best practices. It implements a complete movie management system with directors, built on MongoDB for data persistence and Redis for caching.

### Key Features
-  **Movie Management**: CRUD operations for movies with director relationships
-  **Director Management**: CRUD operations for directors with validation
-  **Redis Caching**: NOT USED, BUT IT HAS THE REQUIRED SETUP
-  **Clean Architecture**: Separation of concerns with dependency inversion
-  **Global Exception Handling**: Structured error responses
-  **Request Validation**: FluentValidation with automatic validation
-  **Unit Tests**: 13 comprehensive unit tests (all passing)
-  **Docker Support**: Full containerization with MongoDB and Redis

## Architecture

```
HubXCase/
├── Movie.Domain/              # Core domain entities and repository interfaces
├── Movie.Application/         # Business logic and application services
├── Movie.Infrastructure/      # External concerns (Database, Caching)
├── Movie.Api/                 # API Controllers and configuration
└── Movie.Api.Tests/          # Unit tests
```

## Technologies

- **.NET 9.0** - Latest .NET framework
- **MongoDB 8.0** - NoSQL database for data storage
- **Redis 7.4** - In-memory cache for performance optimization (NOT IMLEMENTED TO ENDPOINTS)
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Input validation
- **Swagger/OpenAPI** - API documentation
- **xUnit, Moq, FluentAssertions** - Unit testing
- **Docker** - Containerization

## Quick Start with Docker

### Option 1: Using Docker Compose (Recommended)

```bash
# Clone the repository
git clone https://github.com/ClikAdem/HubxCase
cd HubXCase

# Start all services (API, MongoDB, Redis)
docker-compose up -d
```

### Option 2: Manual Docker Commands

If you prefer to run services individually:

```bash
# 1. Start MongoDB
docker run -d --name movie-mongodb -p 27017:27017 -e MONGO_INITDB_DATABASE=MovieDatabase mongo:8.0

# 2. Start Redis
docker run -d --name movie-redis -p 6379:6379 redis:7.4-alpine

# 3. Build Movie API image
docker build -t movie-api -f Movie.Api/Dockerfile .

# 4. Run Movie API container
docker run -d `
  --name movie-api `
  -e ASPNETCORE_ENVIRONMENT=Development `
  -e MongoDbSettings__ConnectionString=mongodb://host.docker.internal:27017 `
  -e MongoDbSettings__DatabaseName=MovieDatabase `
  -e RedisSettings__ConnectionString=host.docker.internal:6379 `
  -e RedisSettings__DefaultExpirationMinutes=60 `
  -p 5000:8080 -p 5001:8081 `
  movie-api
```

## 📚 API Endpoints

The API will be available at:
- **Swagger UI**: http://localhost:5000
- **API**: http://localhost:5000/api
- **Health Check**: http://localhost:5000/health

### Movies
- `GET /api/movies` - Get all movies (would be cached, not implemented)
- `GET /api/movies/{id}` - Get movie by ID
- `POST /api/movies` - Create a new movie
- `PUT /api/movies/{id}` - Update a movie
- `DELETE /api/movies/{id}` - Delete a movie

### Directors
- `GET /api/directors` - Get all directors (would be cached, not implemented)
- `GET /api/directors/{id}` - Get director by ID
- `POST /api/directors` - Create a new director
- `PUT /api/directors/{id}` - Update a director
- `DELETE /api/directors/{id}` - Delete a director

## 🧪 Testing

**Test Results**: 13 unit tests - all passing

## 🔧 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)
