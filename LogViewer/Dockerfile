# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY LogViewer.csproj ./
RUN dotnet restore ./LogViewer.csproj

COPY . ./
RUN dotnet publish ./LogViewer.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish ./


ENTRYPOINT ["dotnet", "LogViewer.dll"]