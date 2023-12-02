FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["web-api/web-api.csproj", "web-api/"]
RUN dotnet restore "web-api/web-api.csproj"
COPY . .
WORKDIR "/src/web-api"
RUN dotnet build "web-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "web-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "web-api.dll"]
