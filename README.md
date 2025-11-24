# Enterprise .NET 10 Reference Architecture 🚀

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4.svg?style=flat&logo=dotnet)](https://dotnet.microsoft.com/download)
[![Build Status](https://github.com/georgepwall1991/CQRSSolution/actions/workflows/dotnet.yml/badge.svg)](https://github.com/georgepwall1991/CQRSSolution/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED.svg?style=flat&logo=docker)](https://www.docker.com/)

A production-ready, high-performance reference implementation demonstrating **Clean Architecture**, **CQRS**, and **Domain-Driven Design (DDD)** using the latest **.NET 10**.

Designed to be the perfect starting point for building scalable, maintainable, and testable enterprise applications.

---

## 🌟 Why Use This Template?

Building enterprise-grade software is hard. This repository provides a battle-tested foundation so you don't have to start from scratch.

*   **🔥 Modern Stack**: Built on the bleeding edge with **.NET 10** and C# 14.
*   **🏗️ Scalable Architecture**: Implements **CQRS** to separate reads from writes, allowing for independent scaling and optimization.
*   **🛡️ Reliable Messaging**: Features the **Transactional Outbox Pattern** to ensure zero data loss when publishing domain events.
*   **⚡ Dual Processing Modes**: flexible Outbox processing via either a simple **Background Service** or scalable **Azure Functions**.
*   **🧪 Test-Driven**: Comes with a comprehensive suite of **Unit** and **Integration** tests using xUnit and Testcontainers-ready architecture.

## ✨ Key Features

| Feature | Description |
| :--- | :--- |
| **Clean Architecture** | Strict separation of concerns (Api -> Application -> Domain <- Infrastructure). |
| **CQRS** | Command Query Responsibility Segregation using **MediatR**. |
| **Domain-Driven Design** | Rich domain models, Value Objects, and decoupled Domain Events. |
| **Reliability** | **Transactional Outbox Pattern** for atomic database operations and event publishing. |
| **Performance** | **Specification Pattern** for efficient, reusable, and testable queries. |
| **Validation** | Automatic request validation pipeline using **FluentValidation**. |
| **Cloud-Ready** | Native integration with **Azure Service Bus** and **Azure Functions**. |
| **Observability** | Built-in **OpenTelemetry** tracing and metrics. |
| **Dockerized** | Full `docker-compose` support for instant local development. |

## 🚀 Quick Start

Get up and running in seconds.

### Prerequisites
*   [Docker Desktop](https://www.docker.com/products/docker-desktop)

### One-Command Launch
We've included a helper script to spin up the API, SQL Server, and the Outbox Processor automatically.

```bash
./start.sh
```

Once started:
*   **API & Swagger**: [http://localhost:7001/swagger](http://localhost:7001/swagger)
*   **Health Check**: [http://localhost:7001/health](http://localhost:7001/health)

## 🛠️ Technology Stack

*   **Core**: .NET 10, C# 14
*   **Web**: ASP.NET Core Web API
*   **Data**: Entity Framework Core (SQL Server)
*   **Messaging**: Azure Service Bus, MediatR
*   **Compute**: Azure Functions (Isolated Worker)
*   **Testing**: xUnit, FluentAssertions, Moq, WebApplicationFactory

## 🏗️ Architecture

The solution follows the principles of Clean Architecture. For a deep dive into the dependency injection strategies and persistence patterns, please read our [**Detailed Architecture Documentation**](docs/ARCHITECTURE.md).

### Folder Structure

*   **`src/CQRSSolution.Api`**: The REST API entry point.
*   **`src/CQRSSolution.Application`**: Business logic, use cases (Commands/Queries).
*   **`src/CQRSSolution.Domain`**: Enterprise logic, Entities, and Domain Events.
*   **`src/CQRSSolution.Infrastructure`**: Database, Service Bus, and external adapters.
*   **`src/CQRSSolution.OutboxProcessor.AzureFunctions`**: Serverless worker for event processing.

## 🏃‍♂️ Manual Setup (No Docker)

If you prefer running locally without Docker Compose:

1.  **Configure Connection Strings**: Update `appsettings.json` in `src/CQRSSolution.Api` with your local SQL Server and Service Bus credentials.
2.  **Apply Migrations**:
    ```bash
    dotnet tool install --global dotnet-ef
    cd src/CQRSSolution.Infrastructure
    dotnet ef database update --startup-project ../CQRSSolution.Api
    ```
3.  **Run API**:
    ```bash
    dotnet run --project src/CQRSSolution.Api
    ```
4.  **Run Tests**:
    ```bash
    dotnet test
    ```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
