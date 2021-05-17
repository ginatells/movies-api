FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /source

# copy .csproj and restore as distinct layers
COPY *.sln .
COPY GinaTellsMovies.API/*.csproj ./GinaTellsMovies.API/
RUN dotnet restore

# copy everything else and build app
COPY GinaTellsMovies.API/. ./GinaTellsMovies.API/
WORKDIR /source/GinaTellsMovies.API
RUN dotnet publish -c Release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "GinaTellsMovies.API.dll"]
