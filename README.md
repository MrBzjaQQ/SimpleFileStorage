# SimpleFileStorage

A simple, modern web application for uploading, listing, downloading, and deleting files. The solution consists of:
- **Backend:** ASP.NET Core (.NET 9) with Entity Framework Core (PostgreSQL)
- **Frontend:** Angular 20 (served as static files from the backend)
- **REST API:** For file management

## Features
- Upload files (with size limit)
- List files (pagination and search)
- Download files
- Delete files
- API documentation via Scalar

## Project Structure
- `SimpleFileStorage.Web/` — ASP.NET Core backend and Angular frontend
- `SimpleFileStorage.Web/ClientApp/SimpleFileStorage.UI/` — Angular frontend source
- `SimpleFileStorage.Tests/` — Integration and service tests

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Node.js & npm](https://nodejs.org/) (for frontend development)
- [Docker](https://www.docker.com/) (optional, for containerized deployment)
- PostgreSQL database

### Backend (ASP.NET Core)
1. Configure your connection string in `SimpleFileStorage.Web/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=files;Username=postgres;Password=postgres"
   }
   ```
2. Run database migrations (if any).
3. Start the backend:
   ```sh
   dotnet run --project SimpleFileStorage.Web
   ```
4. The API and frontend will be available at [http://localhost:8080](http://localhost:8080) (default).

### Frontend (Angular)
For development, see [`SimpleFileStorage.Web/ClientApp/SimpleFileStorage.UI/README.md`](SimpleFileStorage.Web/ClientApp/SimpleFileStorage.UI/README.md):
```sh
cd SimpleFileStorage.Web/ClientApp/SimpleFileStorage.UI
npm install
ng serve
```
Visit [http://localhost:4200](http://localhost:4200) for the Angular dev server.

## API Endpoints
- `POST   /api/files/upload` — Upload a file (multipart/form-data)
- `GET    /api/files/{id}` — Download a file
- `DELETE /api/files/{id}` — Delete a file
- `GET    /api/files` — List files (`take`, `skip`, `searchTerm` query params)

### API Documentation
Interactive docs available via Scalar:
- [http://localhost:8080/scalar/v1](http://localhost:8080/scalar/v1)

## Configuration
See `SimpleFileStorage.Web/appsettings.json` for:
- `DefaultConnection`: PostgreSQL connection string
- `MaxFileSizeKb`: Max upload size (KB)
- `FileStoragePath`: Directory for uploaded files

## Docker
1. Build the image:
   ```sh
   docker build -t simple-file-storage:1.0.0 ./SimpleFileStorage.Web
   ```
2. Run the container:
   ```sh
   docker run -p 8080:8080 --name simplefilestorage \
     -e ASPNETCORE_ENVIRONMENT=Production \
     -e ConnectionStrings__DefaultConnection="Host=<db_host>;Database=files;Username=postgres;Password=postgres" \
     simple-file-storage:1.0.0
   ```
   Replace `<db_host>` as needed.

## Testing
- Backend tests: `dotnet test SimpleFileStorage.Tests`
- Tests cover file upload, download, and delete scenarios using a real PostgreSQL container.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

---
For more details, see the subproject READMEs:
- [`SimpleFileStorage.Web/README.md`](SimpleFileStorage.Web/README.md)
- [`SimpleFileStorage.Web/ClientApp/SimpleFileStorage.UI/README.md`](SimpleFileStorage.Web/ClientApp/SimpleFileStorage.UI/README.md) 