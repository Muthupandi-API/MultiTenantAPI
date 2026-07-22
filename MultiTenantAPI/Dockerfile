## Base runtime image
#FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
#
#WORKDIR /app
#
#EXPOSE 8080
#
#
## Build image
#FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
#
#WORKDIR /src
#
#
## Copy csproj
#COPY MultiTenantAPI.csproj ./
#
#
## Restore
#RUN dotnet restore MultiTenantAPI.csproj
#
#
## Copy source
#COPY . .
#
#
## Build
#RUN dotnet build MultiTenantAPI.csproj -c Release -o /app/build
#
#
## Publish
#FROM build AS publish
#
#RUN dotnet publish MultiTenantAPI.csproj \
    #-c Release \
    #-o /app/publish \
    #/p:UseAppHost=false
#
#
#
## Final image
#FROM base AS final
#
#WORKDIR /app
#
#COPY --from=publish /app/publish .
#
#
#ENTRYPOINT ["dotnet", "MultiTenantAPI.dll"]


# ============================
# Base Runtime Image
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

# ASP.NET Core listens on port 8080
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080


# ============================
# Build Image
# ============================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project file
COPY MultiTenantAPI.csproj ./

# Restore NuGet packages
RUN dotnet restore MultiTenantAPI.csproj

# Copy all source files
COPY . .

# Build the project
RUN dotnet build MultiTenantAPI.csproj \
    -c Release \
    -o /app/build


# ============================
# Publish Image
# ============================
FROM build AS publish

RUN dotnet publish MultiTenantAPI.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# ============================
# Final Runtime Image
# ============================
FROM base AS final

WORKDIR /app

# Copy published files
COPY --from=publish /app/publish .

# Start the application
ENTRYPOINT ["dotnet", "MultiTenantAPI.dll"]