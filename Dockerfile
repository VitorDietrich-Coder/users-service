# =========================
# Base runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar csproj (cache de restore)
COPY Users.Microservice.API/Users.Microservice.API.csproj Users.Microservice.API/
COPY Users.Microservice.Application/Users.Microservice.Application.csproj Users.Microservice.Application/
COPY Users.Microservice.Domain/Users.Microservice.Domain.csproj Users.Microservice.Domain/
COPY Users.Microservice.Infrastructure/Users.Microservice.Infrastructure.csproj Users.Microservice.Infrastructure/

RUN dotnet restore Users.Microservice.API/Users.Microservice.API.csproj

# Copiar todo o código
COPY . .

# Build
WORKDIR /src/Users.Microservice.API
RUN dotnet build Users.Microservice.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish
RUN dotnet publish Users.Microservice.API.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# Migrator (opcional)
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS migrator
WORKDIR /src

COPY . .
WORKDIR /src/Users.Microservice.API

RUN dotnet tool install --global dotnet-ef --version 9.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "ef", "database", "update", "--no-build"]

# =========================
# Runtime final
# =========================
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Users.Microservice.API.dll"]
