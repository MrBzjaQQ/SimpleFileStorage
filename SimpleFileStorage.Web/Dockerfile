FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SimpleFileStorage.Web/SimpleFileStorage.Web.csproj", "SimpleFileStorage.Web/"]
RUN dotnet restore "SimpleFileStorage.Web/SimpleFileStorage.Web.csproj"
COPY . .
WORKDIR "/src/SimpleFileStorage.Web"
RUN dotnet build "SimpleFileStorage.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpleFileStorage.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleFileStorage.Web.dll"]
