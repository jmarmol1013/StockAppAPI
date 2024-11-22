FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/StockAppAPI
COPY ["StockAppAPI.csproj", "./"]
RUN dotnet restore "./StockAppAPI.csproj"
COPY . .
RUN dotnet build "./StockAppAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./StockAppAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockAppAPI.dll"]