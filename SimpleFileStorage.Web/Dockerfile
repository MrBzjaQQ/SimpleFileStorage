# Stage 1: Build Angular app
FROM node:20 AS node-build
WORKDIR /app/ClientApp/SimpleFileStorage.UI
COPY ClientApp/SimpleFileStorage.UI/package*.json ./
RUN npm install
COPY ClientApp/SimpleFileStorage.UI/ .
RUN npm run build -- --output-path=dist

# Stage 2: Build .NET app (with Node.js installed)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
# Install Node.js 20
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs && \
    node -v && npm -v
WORKDIR /src
COPY ["SimpleFileStorage.Web.csproj", "."]
RUN dotnet restore "SimpleFileStorage.Web.csproj"
COPY . .
WORKDIR "/src"
# Copy Angular build output to wwwroot
COPY --from=node-build /app/ClientApp/SimpleFileStorage.UI/dist/browser ./wwwroot
RUN dotnet build "SimpleFileStorage.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish .NET app (Node.js still available)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpleFileStorage.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SimpleFileStorage.Web.dll"]
