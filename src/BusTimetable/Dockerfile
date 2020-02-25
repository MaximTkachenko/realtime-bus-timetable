#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BusTimetable/BusTimetable.csproj", "BusTimetable/"]
COPY ["Models/Models.csproj", "Models/"]
RUN dotnet restore "BusTimetable/BusTimetable.csproj"
COPY . .
WORKDIR "/src/BusTimetable"
RUN dotnet build "BusTimetable.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "BusTimetable.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BusTimetable.dll"]