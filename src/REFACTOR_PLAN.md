# Refactoring and Improvement Plan

This plan details the steps to improve the `CQRSSolution` codebase, focusing on architectural best practices (Clean Architecture, DDD, CQRS), reliability, and testing.

## Goals
1.  **Implement Automated Testing**: Establish a TDD workflow with Unit and Integration tests.
2.  **Strengthen Architecture**: Enforce strict separation of concerns (API -> Application -> Domain <- Infrastructure).
3.  **Improve Domain Model**: Move from anemic models to rich domain models (encapsulation).
4.  **Fix Persistence Logic**: Replace direct `DbContext` usage with Repository/UnitOfWork patterns.
5.  **Enhance Reliability**: Make the Outbox Processor atomic and robust.

## Phase 1: Foundation & Testing (TDD Start)
1.  **Create Branch**: `refactor/improvements-and-tests`.
2.  **Setup Test Projects**:
    *   `CQRSSolution.UnitTests` (xUnit, Moq, FluentAssertions).
    *   `CQRSSolution.IntegrationTests` (xUnit, Testcontainers or In-Memory DB).
3.  **Write Initial Tests**:
    *   Write a failing test for `CreateOrderCommandHandler` (characterizing current behavior).
    *   Write a failing test for `Order` entity validation.

## Phase 2: Architectural Cleanup
1.  **Dependency Injection**:
    *   Update `CQRSSolution.Api/Program.cs` to use `CQRSSolution.Infrastructure.DependencyInjection` instead of manual registration.
    *   Register MediatR `IPipelineBehavior` for FluentValidation in `CQRSSolution.Application`.
2.  **Refactor Command Handler**:
    *   **Goal**: Remove `IApplicationDbContext` and manual transaction logic from `CreateOrderCommandHandler`.
    *   **Step**: Inject `IOrderRepository` and `IUnitOfWork`.
    *   **Step**: Rely on the Validation Pipeline for input validation.
    *   **Step**: Rely on Domain Events -> Outbox (handled by Repository/UnitOfWork) for messaging. *Note: This requires ensuring the Repository/UoW handles the outbox saving, or using a Domain Event Handler that saves to Outbox.*

## Phase 3: Domain Model Refactoring
1.  **Encapsulate `Order`**:
    *   Make setters private.
    *   Remove `[Key]`, `[Required]` attributes (move to `Infrastructure/Persistence/Configurations`).
    *   Ensure strict invariants in the constructor/factory methods.
2.  **Update Infrastructure**:
    *   Create `OrderConfiguration` (IEntityTypeConfiguration).
    *   Ensure `ApplicationDbContext` applies configurations.

## Phase 4: Reliability (Outbox)
1.  **Fix `OutboxProcessorService`**:
    *   Ensure atomicity (Process -> Update DB) or Idempotency.
    *   Refactor to avoid Service Locator pattern if possible.

## Phase 5: Verification
1.  Run all tests.
2.  Verify API runs locally.

## Execution Strategy
We will follow **TDD**. Before refactoring a component, we will write a test that ensures the current behavior (or the desired behavior) is captured.
