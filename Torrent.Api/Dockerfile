#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Torrent.Api/Torrent.Api.csproj", "Torrent.Api/"]
COPY ["Torrent.Infrastructure/Torrent.Infrastructure/Torrent.Infrastructure.csproj", "Torrent.Infrastructure/Torrent.Infrastructure/"]
COPY ["Torrent.Application/Torrent.Application.csproj", "Torrent.Application/"]
COPY ["Torrent.Core/Torrent.Core.csproj", "Torrent.Core/"]
COPY ["Torrent.Storage/Torrent.Storage.csproj", "Torrent.Storage/"]
RUN dotnet restore "./Torrent.Api/Torrent.Api.csproj"
COPY . .
WORKDIR "/src/Torrent.Api"
RUN dotnet build "./Torrent.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Torrent.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Torrent.Api.dll"]