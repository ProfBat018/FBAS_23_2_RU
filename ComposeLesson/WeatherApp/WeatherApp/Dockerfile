# STAGE 1: Migration tooling
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS migration
WORKDIR /app
EXPOSE 5001

ARG CONNECTION_STRING
ENV CONNECTION_STRING=$CONNECTION_STRING

COPY . .
RUN dotnet restore
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# STAGE 2: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet publish -c Release -o /app/publish

# STAGE 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WeatherApp.dll"]