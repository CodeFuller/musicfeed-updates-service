# docker build -t musicfeed/updates-service:latest -f Dockerfile ../..

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY ["src/MusicFeed.UpdatesService/MusicFeed.UpdatesService.csproj", "/project/src/MusicFeed.UpdatesService/"]
# MusicFeed.UpdatesService.Client is not published within an image, but this project is used by UpdatesService.IntegrationTests
COPY ["src/MusicFeed.UpdatesService.Client/MusicFeed.UpdatesService.Client.csproj", "/project/src/MusicFeed.UpdatesService.Client/"]
COPY ["tests/MusicFeed.UpdatesService.IntegrationTests/MusicFeed.UpdatesService.IntegrationTests.csproj", "/project/tests/MusicFeed.UpdatesService.IntegrationTests/"]
COPY ["MusicFeed.UpdatesService.sln", "/project/"]
RUN dotnet restore "/project/MusicFeed.UpdatesService.sln"

COPY . "/project/"

RUN dotnet build "/project/MusicFeed.UpdatesService.sln" --no-restore -c Release

RUN dotnet test "/project/MusicFeed.UpdatesService.sln" --no-build -c Release --logger "trx;LogFileName=MusicFeed.UpdatesService.trx" 

FROM build AS publish
RUN dotnet publish "/project/src/MusicFeed.UpdatesService/MusicFeed.UpdatesService.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish /app

ENTRYPOINT ["dotnet", "MusicFeed.UpdatesService.dll"]
