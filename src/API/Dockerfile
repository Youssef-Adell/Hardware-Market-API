# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy project files and restore first to take advantage of docker layer caching (This avoids re-downloading and restoring all the NuGet packages in future builds)
COPY src/HardwareMarket.sln ./
COPY src/API/API.csproj API/
COPY src/Core/Core.csproj Core/
COPY src/Infrastructure/Infrastructure.csproj Infrastructure/

# Restore dependencies for all projects
RUN dotnet restore

# Copy the rest of the source code and build/publish the Web API
COPY src/. .
RUN dotnet publish API/API.csproj -c Release -o /publish


# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /release

# Copy published files from the build stage
COPY --from=build /publish ./

# Run the API
ENTRYPOINT ["dotnet", "API.dll"]
