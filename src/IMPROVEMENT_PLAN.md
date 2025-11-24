# Improvement Plan

This document outlines a strategic plan to elevate the `CQRSSolution` from a prototype to a production-ready enterprise reference implementation.

## 1. DevOps & Developer Experience 🐳
**Goal:** Simplify onboarding and ensure consistent environments.

*   **Docker Integration:**
    *   Create `Dockerfile` for `CQRSSolution.Api` and `CQRSSolution.OutboxProcessor.AzureFunctions`.
    *   Create `docker-compose.yml` to orchestrate:
        *   The API
        *   SQL Server (Linux container)
        *   Azure Service Bus Emulator (or RabbitMQ as a dev alternative)
        *   Zipkin/Jaeger (for tracing)
*   **CI/CD Pipelines:**
    *   Add GitHub Actions workflow for:
        *   Build & Test (Unit + Integration).
        *   Code Style Check (`dotnet format`).

## 2. Observability & Monitoring 🔭
**Goal:** Gain visibility into distributed transactions.

*   **OpenTelemetry:**
    *   Implement OpenTelemetry in API and Functions.
    *   Instrument EF Core, HTTP Client, and Service Bus.
    *   Export traces to Zipkin or Jaeger.
    *   Export metrics to Prometheus.
*   **Health Checks:**
    *   Implement standard .NET Health Checks (`/health`, `/health/ready`).
    *   Add probes for Database and Service Bus connectivity.
*   **Structured Logging:**
    *   Ensure all logs are structured (JSON) for easy parsing by aggregators like ELK or Seq.

## 3. Architecture & Code Quality 🏗
**Goal:** Enforce architectural rules and improve maintainability.

*   **Architecture Tests:**
    *   Introduce `NetArchTest` rules in `CQRSSolution.UnitTests`.
    *   Enforce:
        *   `Domain` has no dependencies.
        *   `Application` depends only on `Domain`.
        *   `Infrastructure` depends on `Application` and `Domain`.
        *   Controllers do not reference `DbContext` directly.
*   **Static Analysis:**
    *   Add `.editorconfig` for consistent coding styles.
    *   Integrate Roslyn analyzers.

## 4. Security 🔒
**Goal:** Secure the API and infrastructure.

*   **Authentication & Authorization:**
    *   Implement JWT Authentication (e.g., using a mock Identity Provider or Auth0).
    *   Add `[Authorize]` attributes to `OrdersController`.
    *   Implement Policy-based authorization if needed.
*   **Secrets Management:**
    *   Ensure no secrets are committed to git (enforce User Secrets for local dev).

## 5. Resilience & Reliability 🛡
**Goal:** Ensure the system handles failures gracefully.

*   **Polly Integration:**
    *   Add Retry and Circuit Breaker policies for:
        *   Database connections (transient errors).
        *   Service Bus publishing.
*   **Idempotency:**
    *   Ensure message consumers (if added) are idempotent.
    *   Implement Idempotency Keys for the API `POST` endpoints to prevent duplicate orders on retry.

## 6. Feature Enhancements ✨
**Goal:** Add "Enterprise" features.

*   **API Versioning:**
    *   Implement `Microsoft.AspNetCore.Mvc.Versioning`.
*   **Caching:**
    *   Implement Distributed Caching (Redis) for Query handlers (e.g., `GetOrderById`).
*   **Documentation:**
    *   Enhance Swagger with XML comments (already started).
    *   Generate architecture diagrams (C4 model).
