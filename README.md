# Notes API

Backend API for Notes Application built with .NET 10, Dapper, EF Core (for migrations), and Microsoft SQL Server.

---

## Database (Docker MSSQL)

### Start Container

```bash
docker start mssql-server
```

If setting up for the first time:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=dev@12345" -p 1433:1433 --name mssql-server -d mcr.microsoft.com/mssql/server:2022-latest
```

### Connection Details

- **Host**: `localhost,1433`
- **Database**: `NotesDb`
- **User**: `sa`
- **Password**: `dev@12345`
- **Connection String**: `Server=localhost,1433;Database=NotesDb;User Id=sa;Password=dev@12345;TrustServerCertificate=True;`

---

## Getting Started

### 1. Prerequisites

- .NET 10 SDK
- Docker Desktop
- EF Core CLI tool:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### 2. Apply Migrations (First Time)

```bash
dotnet ef database update
```

### 3. Run the API

```bash
dotnet run
```

API runs on `http://localhost:3000`.

---

## Database Commands

| Command              | Action                                              |
| :------------------- | :-------------------------------------------------- |
| `dotnet run`         | Start API server (no auto-seeding)                  |
| `dotnet run --seed`  | Seed default users and sample notes                 |
| `dotnet run --reset` | Clear all data, reset identity counter, and re-seed |

### Default Users

- **Username**: `techbodia` | **Password**: `admin@12345`
- **Username**: `seanghor` | **Password**: `admin@12345`

---

## Migrations (When Changing Models)

When adding or modifying fields in `Models/Note.cs` or `Models/User.cs`:

1. Update the Model and ensure it is in `Data/AppDbContext.cs`.
2. Add a new migration:
   ```bash
   dotnet ef migrations add <MigrationName>
   ```
3. Apply migration to the database:
   ```bash
   dotnet ef database update
   ```
4. Update repository SQL queries in `Repositories/` and request/response DTOs in `DTOs/`.

To rollback:

```bash
dotnet ef database update <PreviousMigrationName>
```

---

## API Endpoints

### Auth (`/api/auth`)

- `POST /api/auth/register` — Register user
- `POST /api/auth/login` — Login & get token

### Notes (`/api/notes`) — _Requires Authorization: Bearer `<token>`_

- `GET /api/notes/list` — Get notes (`?search=...&category=...&sortOrder=asc|desc`)
- `GET /api/notes/getByID/{id}` — Get note by ID
- `POST /api/notes/create` — Create note
- `PUT /api/notes/updateByID/{id}` — Update note
- `DELETE /api/notes/deleteByID/{id}` — Delete note

### Users (`/api/users`)

- `GET /api/users/list` — List users

### Categories

`Personal`, `Work`, `Study`, `Ideas`, `Todo`
