using CQRSSolution.Api;
using CQRSSolution.Application.Commands.CreateOrder;
using CQRSSolution.Application.DTOs;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace CQRSSolution.IntegrationTests.Controllers;

public class OrdersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public OrdersControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateOrder_ReturnsCreated_WhenRequestIsValid()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new CreateOrderCommand
        {
            CustomerName = "Test Customer",
            CustomerEmail = "test@example.com",
            Items = new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto
                {
                    ProductName = "Test Product",
                    Quantity = 1,
                    UnitPrice = 10.0m
                }
            }
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/orders", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var result = await response.Content.ReadFromJsonAsync<CreateOrderResponse>();
        result.Should().NotBeNull();
        result!.OrderId.Should().NotBeEmpty();
    }

    private class CreateOrderResponse
    {
        public Guid OrderId { get; set; }
    }
}
