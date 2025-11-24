using MediatR;
using System.Text.Json;
using CQRSSolution.Application.DTOs;
using CQRSSolution.Application.Factories;
using CQRSSolution.Application.Interfaces;
using CQRSSolution.Domain.Entities;
using CQRSSolution.Domain.DomainEvents;
using Microsoft.Extensions.Logging;

namespace CQRSSolution.Application.Commands.CreateOrder;

/// <summary>
///     Handles the <see cref="CreateOrderCommand" /> to create a new order for a customer.
///     This handler orchestrates finding or creating a customer, creating the order and its items,
///     and ensuring an <see cref="OrderCreatedDomainEvent" /> is stored as an <see cref="OutboxMessage" />.
///     All database operations are performed within a single atomic transaction managed by <see cref="IUnitOfWork" />.
/// </summary>
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderFactory _orderFactory;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateOrderCommandHandler" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for managing transactions and repositories.</param>
    /// <param name="orderFactory">The factory for creating orders.</param>
    /// <param name="logger">The logger.</param>
    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IOrderFactory orderFactory, ILogger<CreateOrderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _orderFactory = orderFactory ?? throw new ArgumentNullException(nameof(orderFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false // Compact JSON for storage
        };
    }

    /// <summary>
    ///     Handles the command to create an order.
    /// </summary>
    /// <param name="command">The <see cref="CreateOrderCommand" /> containing order details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>The ID of the newly created order.</returns>
    public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        // Validation is handled by the ValidationBehavior pipeline.

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // 1. Get or Create Customer
            var customer = await _unitOfWork.Customers.GetByEmailAsync(command.CustomerEmail, cancellationToken);
            if (customer == null)
            {
                customer = Customer.Create(command.CustomerName, command.CustomerEmail);
                await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
                _logger.LogInformation("Creating new customer: {CustomerName} ({CustomerEmail})", command.CustomerName, command.CustomerEmail);
            }
            else
            {
                _logger.LogInformation("Found existing customer: {CustomerName} ({CustomerEmail})", customer.Name, customer.Email);
            }

            // 2. Create Order
            var orderItemsDto = command.Items.Select(i => new OrderItemDto
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            var order = _orderFactory.CreateNewOrder(customer, orderItemsDto);

            await _unitOfWork.Orders.AddAsync(order, cancellationToken);

            // 3. Create Outbox Message
            var orderCreatedEvent = new OrderCreatedDomainEvent(
                order.OrderId,
                order.CustomerName,
                order.TotalAmount,
                order.OrderDate
            );

            var outboxMessage = new OutboxMessage(
                orderCreatedEvent.GetType().FullName ?? nameof(OrderCreatedDomainEvent),
                JsonSerializer.Serialize(orderCreatedEvent, _jsonSerializerOptions)
            );

            await _unitOfWork.OutboxMessages.AddAsync(outboxMessage, cancellationToken);

            // 4. Save and Commit
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            _logger.LogInformation("Order {OrderId} created successfully for customer {CustomerName}.", order.OrderId, order.CustomerName);

            return order.OrderId;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error creating order for customer {CustomerName}. Details: {ErrorMessage}", command.CustomerName, ex.Message);
            throw;
        }
    }
}