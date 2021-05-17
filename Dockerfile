#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5011

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["OMDB-Service.csproj", ""]
RUN dotnet restore "./OMDB-Service.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "OMDB-Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OMDB-Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OMDB-Service.dll"]
