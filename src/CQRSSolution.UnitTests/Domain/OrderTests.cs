using CQRSSolution.Domain.Entities;
using CQRSSolution.Domain.Enums;
using FluentAssertions;
using System;
using Xunit;

namespace CQRSSolution.UnitTests.Domain;

public class OrderTests
{
    [Fact]
    public void AddOrderItem_Should_Add_Item_And_Update_TotalAmount()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), "Test Customer", DateTime.UtcNow, OrderStatus.Pending);
        var initialTotal = order.TotalAmount;

        // Act
        order.AddOrderItem("Product A", 2, 10.0m); // 2 * 10 = 20

        // Assert
        order.OrderItems.Should().HaveCount(1);
        order.TotalAmount.Should().Be(initialTotal + 20.0m);
    }

    [Fact]
    public void AddOrderItem_Should_Throw_ArgumentException_When_Quantity_Is_Zero()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), "Test Customer", DateTime.UtcNow, OrderStatus.Pending);

        // Act & Assert
        order.Invoking(o => o.AddOrderItem("Product A", 0, 10.0m))
            .Should().Throw<ArgumentException>()
             .WithParameterName("quantity");
    }
}
