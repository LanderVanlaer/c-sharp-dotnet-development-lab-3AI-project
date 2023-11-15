FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["c-sharp-dotnet-development-lab-3AI-project/c-sharp-dotnet-development-lab-3AI-project.csproj", "c-sharp-dotnet-development-lab-3AI-project/"]
RUN dotnet restore "c-sharp-dotnet-development-lab-3AI-project/c-sharp-dotnet-development-lab-3AI-project.csproj"
COPY . .
WORKDIR "/src/c-sharp-dotnet-development-lab-3AI-project"
RUN dotnet build "c-sharp-dotnet-development-lab-3AI-project.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "c-sharp-dotnet-development-lab-3AI-project.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "c-sharp-dotnet-development-lab-3AI-project.dll"]
