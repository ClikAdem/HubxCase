FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Movie.Api/Movie.Api.csproj", "Movie.Api/"]
COPY ["Movie.Application/Movie.Application.csproj", "Movie.Application/"]
COPY ["Movie.Domain/Movie.Domain.csproj", "Movie.Domain/"]
COPY ["Movie.Infrastructure/Movie.Infrastructure.csproj", "Movie.Infrastructure/"]

RUN dotnet restore "Movie.Api/Movie.Api.csproj"

COPY . .

WORKDIR "/src/Movie.Api"
RUN dotnet build "Movie.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Movie.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Movie.Api.dll"]