# SimpleFileStorage.Web

## Overview

**SimpleFileStorage.Web** is a web application for uploading, listing, downloading, and deleting files. It features:
- A backend built with ASP.NET Core (.NET 9) and Entity Framework Core (PostgreSQL).
- A modern Angular frontend (Angular 20).
- REST API endpoints for file management.
- File metadata is stored in a PostgreSQL database, while file contents are stored on disk.

## Features

- **Upload files** (with size limit)
- **List files** (with pagination and search)
- **Download files**
- **Delete files**
- **API documentation via Scalar** (see below)

## Configuration

The main configuration file is `appsettings.json`. Key parameters:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=host.docker.internal;Database=files;Username=postgres;Password=postgres"
  },
  "StorageSettings": {
    "MaxFileSizeKb": 2048,
    "FileStoragePath": "Uploads"
  }
}
```

- **DefaultConnection**: PostgreSQL connection string.
- **MaxFileSizeKb**: Maximum allowed file size for uploads (in KB).
- **FileStoragePath**: Directory for storing uploaded files.

## Running with Docker

### Prerequisites

- Docker
- PostgreSQL database (running and accessible to the container)

### Build and Run

1. **Build the Docker image:**

   ```sh
   docker build -t simple-file-storage:1.0.0 .
   ```

2. **Run the application:**

   ```sh
   docker run -p 8080:8080 --name simplefilestorage \
     -e ASPNETCORE_ENVIRONMENT=Production \
     -e ConnectionStrings__DefaultConnection="Host=<db_host>;Database=files;Username=postgres;Password=postgres" \
     simple-file-storage:1.0.0
   ```

   - Replace `<db_host>` with your PostgreSQL host (e.g., `host.docker.internal` for local development).

3. **Access the app:**
   - Open [http://localhost:8080](http://localhost:8080) in your browser.

## API

- **POST** `/api/files/upload` — Upload a file
- **GET** `/api/files/{id}` — Download a file
- **DELETE** `/api/files/{id}` — Delete a file
- **GET** `/api/files` — List files (supports `take`, `skip`, `searchTerm` query params)

## API Documentation (Scalar)

Interactive API documentation is available via [Scalar](https://scalar.com/):
- Visit [http://localhost:8080/scalar/v1](http://localhost:8080/scalar/v1) (or the corresponding path on your deployment)

## Frontend

The Angular frontend is built and served as static files from the backend. For development, see `ClientApp/SimpleFileStorage.UI/README.md`. 