# docker build -t musicfeed/updates-service:latest -f "../../src/MusicFeed.UpdatesService/Dockerfile" ../..

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /build

COPY ["src/MusicFeed.UpdatesService/MusicFeed.UpdatesService.csproj", "src/MusicFeed.UpdatesService/"]
# MusicFeed.UpdatesService.Client is not published within an image, but this project is used by integration tests.
COPY ["src/MusicFeed.UpdatesService.Client/MusicFeed.UpdatesService.Client.csproj", "src/MusicFeed.UpdatesService.Client/"]
COPY ["tests/MusicFeed.UpdatesService.IntegrationTests/MusicFeed.UpdatesService.IntegrationTests.csproj", "tests/MusicFeed.UpdatesService.IntegrationTests/"]
COPY ["MusicFeed.UpdatesService.Build.sln", "./"]
COPY ["MusicFeed.UpdatesService.src.ruleset", "./"]
COPY ["MusicFeed.UpdatesService.tests.ruleset", "./"]

RUN dotnet restore "MusicFeed.UpdatesService.Build.sln"

COPY . .

RUN dotnet restore "MusicFeed.UpdatesService.Build.sln"

RUN dotnet build "MusicFeed.UpdatesService.Build.sln" --no-restore --configuration Release

FROM build AS test

# We don't let dotnet test command to fail build if some tests are failed.
# This will prevent publish of tests results (trx file).
# If some tests are failed, the task PublishTestResults will fail due to failTaskOnFailedTests parameter.
RUN dotnet test "MusicFeed.UpdatesService.Build.sln" --no-build --configuration Release --logger "trx" --collect:"XPlat Code Coverage"; exit 0

RUN mkdir TestResults
RUN cp -r --target-directory=TestResults --parents tests/*/TestResults

FROM test AS publish
RUN dotnet publish "src/MusicFeed.UpdatesService/MusicFeed.UpdatesService.csproj" --no-build --configuration Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicFeed.UpdatesService.dll"]
