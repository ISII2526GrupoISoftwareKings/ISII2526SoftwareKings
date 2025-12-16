# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AppForSEII2526.API.csproj ./
RUN dotnet restore ./AppForSEII2526.API.csproj

COPY . ./
RUN dotnet publish ./AppForSEII2526.API.csproj -c Release -o /app/publish --no-restore
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish ./


ENTRYPOINT ["dotnet", "AppForSEII2526.API.dll"]