# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RentalCar.Category.Api/RentalCar.Categories.Api.csproj", "RentalCar.Category.Api/"]
COPY ["RentalCar..Category.Application/RentalCar.Categories.Application.csproj", "RentalCar..Category.Application/"]
COPY ["RentalCar..Category.Core/RentalCar.Categories.Core.csproj", "RentalCar..Category.Core/"]
COPY ["RentalCar..Category.Infrastructure/RentalCar.Categories.Infrastructure.csproj", "RentalCar..Category.Infrastructure/"]
RUN dotnet restore "./RentalCar.Category.Api/RentalCar.Categories.Api.csproj"
COPY . .
WORKDIR "/src/RentalCar.Category.Api"
RUN dotnet build "./RentalCar.Categories.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RentalCar.Categories.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RentalCar.Categories.Api.dll"]