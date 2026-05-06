# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY backend/TechClient.sln .
COPY backend/TechClient.API/TechClient.API.csproj TechClient.API/
COPY backend/TechClient.Application/TechClient.Application.csproj TechClient.Application/
COPY backend/TechClient.Domain/TechClient.Domain.csproj TechClient.Domain/
COPY backend/TechClient.Infrastructure/TechClient.Infrastructure.csproj TechClient.Infrastructure/

RUN dotnet restore "TechClient.API/TechClient.API.csproj"

COPY backend/. .
RUN dotnet publish TechClient.API/TechClient.API.csproj \
    -c Release \
    -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

RUN useradd -m appuser
COPY --from=build /app/publish .
COPY --from=build /src/TechClient.API/credentials ./credentials

RUN chown -R appuser:appuser /app
USER appuser

EXPOSE 8080
ENTRYPOINT ["dotnet", "TechClient.API.dll"]