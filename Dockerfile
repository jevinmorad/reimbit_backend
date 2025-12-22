# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Start/Start.csproj", "Start/"]
COPY ["Reimbit/Reimbit.Application/Reimbit.Application.csproj", "Reimbit/Reimbit.Application/"]
COPY ["Reimbit/Reimbit.Contracts/Reimbit.Contracts.csproj", "Reimbit/Reimbit.Contracts/"]
COPY ["Reimbit/Reimbit.Domain/Reimbit.Domain.csproj", "Reimbit/Reimbit.Domain/"]
COPY ["Reimbit/Reimbit.Infrastructure/Reimbit.Infrastructure.csproj", "Reimbit/Reimbit.Infrastructure/"]
COPY ["Reimbit/Reimbit.Web/Reimbit.Web.csproj", "Reimbit/Reimbit.Web/"]
RUN dotnet restore "./Start/Start.csproj"
COPY . .
WORKDIR "/src/Start"
RUN dotnet build "./Start.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Start.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Start.dll"]