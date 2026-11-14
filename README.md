# Enterprise Secure Proxy

## Overview

Enterprise Secure Proxy is a secure API gateway/proxy 
that validates incoming requests, 
enforces operation and destination configuration, 
forwards approved requests to configured downstream 
services, and provides auditing and security controls.

Key features:

- Operation-based request routing
- AES-GCM encrypted payload support
- Request validation
- Destination management
- Audit logging
- Concurrency protection
- Secure error handling
- Integration test coverage

---

## Architecture

The solution follows a layered architecture:

- API Layer
- Application Layer
- Infrastructure Layer
- Persistence Layer
- Integration Tests

Key architectural patterns:

- Dependency Injection
- Repository Pattern
- Service Layer
- Configuration-driven routing
- Entity Framework Core with PostgreSQL/SQL Server support

---

## Prerequisites

Required software:

- .NET 8 SDK
- Visual Studio 2022
- PostgreSQL/SQL Server (for database)
- Git

Recommended:

- Postman
- pgAdmin (for PostgreSQL)
- SQL Server Management Studio (for SQL Server)

---

## Database Setup

Update the connection string in:

```json
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "ProxyDb": "Host=localhost;Port=5432;Database=proxydb;Username=proxyuser;Password=ChangeMe",
    "SqlServerConnection": "Server=localhost;Database=SqlServerEnterpriseProxyDB;User Id=sa;Password=ChangeMe;"
  }
}
```

Apply database migrations:

For PostgreSQL:
```bash
dotnet ef database update
```

If SQL scripts are provided:

```sql
-- Execute scripts from /Database/Scripts
```
Can select in Programm.cs SQL Server or PostgreSQL
- Uses Entity Framework (code-first) 
- Stores  data in SQL Server (PostgreSQL) 

In the Operation entity, the concurrency token is implemented as follows:

SQL Server
[Timestamp]
public byte[] RowVersion { get; set; }

PostgreSQL concurrency token
public uint xmin { get; private set; }

---

## Configuration

Configuration is stored in:

```text
appsettings.json
appsettings.Development.json
```

Important configuration areas:

- Database connection strings
- Destinations
- Cache settings
- Authentication
- Encryption settings

---

## Building the Solution

Restore packages:

```bash
dotnet restore
```

Build:

```bash
dotnet build
```

---

## Running the Application

Start the API:

```bash
dotnet run
```

The Swagger UI will be available at:

```text
https://localhost:<port>/swagger
```

---

## Running Automated Tests

Run all integration tests:

```bash
dotnet test tests/IntegrationTests/IntegrationTests.csproj
```

Latest test result:

```text
Total:     23
Passed:    17
Skipped:    6
Failed:     0
```

---

## Test Coverage

Implemented integration tests include:

- Startup validation
- Unknown operation handling
- Disabled operation handling
- Disabled destination handling
- Missing payload validation
- Missing parameter validation
- Invalid encrypted payload validation
- Invalid HTTP method handling
- Path traversal protection
- Concurrency conflict handling
- Audit record creation
- Correlation ID logging
- Audit duration logging
- Audit outcome logging
- Error handling

Additional tests are present for:

- Forwarding verification
- Header filtering
- Destination override protection
- Upstream timeout handling

These are currently skipped because they require dedicated forwarding infrastructure or active destination configuration.

---

## Security Features

The proxy includes:

- AES-GCM encryption support
- Request validation
- Destination validation
- Operation validation
- Secure error responses
- Audit logging
- Correlation IDs
- Path traversal protection

---

## Known Limitations

The following tests are currently skipped:

- Forwarding verification
- Header filtering
- Destination override protection
- Upstream timeout handling

These require dedicated test infrastructure and/or active destination configuration.

---

## Repository

Source code, documentation and test assets are included in this repository.

Contents:

- Source code
- Automated tests
- Database scripts / migrations
- README.md
- DEFECT-REPORT.md
- DESIGN-NOTES.md
- AI usage declaration
