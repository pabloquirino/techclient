FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY backend/TechClient.sln .
COPY backend/TechClient.API/TechClient.API.csproj TechClient.API/
COPY backend/TechClient.Application/TechClient.Application.csproj TechClient.Application/
COPY backend/TechClient.Domain/TechClient.Domain.csproj TechClient.Domain/
COPY backend/TechClient.Infrastructure/TechClient.Infrastructure.csproj TechClient.Infrastructure/

RUN dotnet restore

COPY backend/. .
RUN dotnet publish TechClient.API/TechClient.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechClient.API.dll"]