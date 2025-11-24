# CQRSSolution

A robust .NET solution demonstrating **Clean Architecture** and **CQRS** (Command Query Responsibility Segregation) principles. This project serves as a reference implementation for building scalable, maintainable, and testable enterprise applications using modern .NET technologies.

## 🚀 Features

- **CQRS Pattern**: Segregation of read (Queries) and write (Commands) operations using **MediatR**.
- **Clean Architecture**: Strict separation of concerns with distinct layers (Api, Application, Domain, Infrastructure).
- **Transactional Outbox Pattern**: Ensures reliable event delivery by atomically saving domain events with business data.
- **Specification Pattern**: Encapsulates query logic to create reusable and testable query specifications.
- **Repository & Unit of Work**: Abstraction over data access to ensure transactional integrity.
- **Dual Outbox Processing**: Flexible implementation offering two ways to process outbox messages:
   - **In-Process**: Using a .NET `BackgroundService`.
   - **Serverless**: Using an **Azure Function** for independent scaling.
- **Domain Events**: Decoupled business logic using internal domain events.
- **Validation**: Request validation using **FluentValidation**.
- **Azure Service Bus**: Integration for asynchronous messaging.

## 🏗 Architecture

The solution is organized into the following projects:

- **`CQRSSolution.Api`**: The entry point (REST API). Handles HTTP requests and dispatches commands/queries.
- **`CQRSSolution.Application`**: Contains business logic, commands, queries, handlers, validators, and interfaces. Depends only on the Domain.
- **`CQRSSolution.Domain`**: The core of the solution. Contains entities, value objects, enums, and domain events. No external dependencies.
- **`CQRSSolution.Infrastructure`**: Implements interfaces defined in Application. Handles data access (EF Core), messaging (Azure Service Bus), and background processing.
- **`CQRSSolution.OutboxProcessor.AzureFunctions`**: A separate Azure Functions project for processing the Outbox table in a serverless environment.

## 🛠 Technologies

- **.NET 8** (or latest supported version)
- **Entity Framework Core** (SQL Server)
- **MediatR**
- **Azure Service Bus**
- **Azure Functions**
- **FluentValidation**
- **xUnit** (Integration & Unit Tests)

## 📋 Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or Docker)
- [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/) (Namespace & Queue/Topic) or an emulator.

## ⚙️ Getting Started

### 1. Configuration

**API (`CQRSSolution.Api/appsettings.json`):**

Update the connection strings and Service Bus settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=CQRSSolutionDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "AzureServiceBus": {
    "ConnectionString": "<YOUR_SERVICE_BUS_CONNECTION_STRING>",
    "TopicName": "orders-topic"
  }
}
```

**Azure Functions (`CQRSSolution.OutboxProcessor.AzureFunctions/local.settings.json`):**

If running the serverless outbox processor:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "SqlDatabaseConnectionString": "Server=(localdb)\mssqllocaldb;Database=CQRSSolutionDb;Trusted_Connection=True;MultipleActiveResultSets=true",
    "ServiceBus:ConnectionString": "<YOUR_SERVICE_BUS_CONNECTION_STRING>"
  }
}
```

### 2. Database Setup

Apply the Entity Framework Core migrations to create the database schema:

```bash
dotnet tool install --global dotnet-ef
cd src/CQRSSolution.Infrastructure
dotnet ef database update --startup-project ../CQRSSolution.Api
```

### 3. Running the Application

**Run the API:**

```bash
cd src/CQRSSolution.Api
dotnet run
```
The API will be available at `https://localhost:7001` (or configured port). Swagger UI is available at `/swagger`.

**Run the Azure Function (Optional):**

```bash
cd src/CQRSSolution.OutboxProcessor.AzureFunctions
func start
```

## 🧪 Testing

The solution includes both unit and integration tests.

**Run all tests:**

```bash
dotnet test
```

- **Unit Tests**: `CQRSSolution.UnitTests` focuses on domain logic and individual components.
- **Integration Tests**: `CQRSSolution.IntegrationTests` uses `WebApplicationFactory` to test the full pipeline including database interactions.
