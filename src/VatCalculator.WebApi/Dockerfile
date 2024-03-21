#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/VatCalculator.WebApi/VatCalculator.WebApi.csproj", "src/VatCalculator.WebApi/"]
COPY ["src/VatCalculator.Application/VatCalculator.Application.csproj", "src/VatCalculator.Application/"]
COPY ["src/VatCalculator.Domain/VatCalculator.Domain.csproj", "src/VatCalculator.Domain/"]
RUN dotnet restore "./src/VatCalculator.WebApi/VatCalculator.WebApi.csproj"
COPY . .
WORKDIR "/src/src/VatCalculator.WebApi"
RUN dotnet build "./VatCalculator.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VatCalculator.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VatCalculator.WebApi.dll"]