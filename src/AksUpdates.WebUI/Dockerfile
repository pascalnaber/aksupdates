#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AksUpdates.WebUI/AksUpdates.WebUI.csproj", "AksUpdates.WebUI/"]
RUN dotnet restore "AksUpdates.WebUI/AksUpdates.WebUI.csproj"
COPY . .
WORKDIR "/src/AksUpdates.WebUI"
RUN dotnet build "AksUpdates.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AksUpdates.WebUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AksUpdates.WebUI.dll"]