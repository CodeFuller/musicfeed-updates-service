FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

COPY ["src/UpdatesService/UpdatesService.csproj", "/project/src/UpdatesService/"]
# UpdatesService.Client is not published within an image, but this project is used by UpdatesService.IntegrationTests
COPY ["src/UpdatesService.Client/UpdatesService.Client.csproj", "/project/src/UpdatesService.Client/"]
COPY ["tests/UpdatesService.IntegrationTests/UpdatesService.IntegrationTests.csproj", "/project/tests/UpdatesService.IntegrationTests/"]
COPY ["UpdatesService.sln", "/project/"]
RUN dotnet restore "/project/UpdatesService.sln"

COPY . "/project/"

RUN dotnet build "/project/UpdatesService.sln" --no-restore -c Release

RUN dotnet test "/project/UpdatesService.sln" --no-build -c Release --logger "trx;LogFileName=UpdatesService.trx" 

FROM build AS publish
RUN dotnet publish "/project/src/UpdatesService/UpdatesService.csproj" --no-restore -c Release -o /app/publish

RUN GRPC_HEALTH_PROBE_VERSION=v0.3.1 && \
    wget -qO/bin/grpc_health_probe https://github.com/grpc-ecosystem/grpc-health-probe/releases/download/${GRPC_HEALTH_PROBE_VERSION}/grpc_health_probe-linux-amd64 && \
    chmod +x /bin/grpc_health_probe

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish /app
COPY --from=publish /bin/grpc_health_probe /bin/grpc_health_probe

ENTRYPOINT ["dotnet", "UpdatesService.dll"]
