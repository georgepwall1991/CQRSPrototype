using System;

namespace CQRSSolution.Domain.Entities;

/// <summary>
///     Represents an item within an order.
/// </summary>
public class OrderItem
{
    /// <summary>
    ///     Gets or sets the unique identifier for the order item.
    /// </summary>
    public Guid OrderItemId { get; private set; }

    /// <summary>
    ///     Gets or sets the foreign key referencing the Order.
    /// </summary>
    public Guid OrderId { get; private set; }

    /// <summary>
    ///     Gets or sets the navigation property to the Order.
    ///     This is virtual to enable lazy loading by EF Core.
    /// </summary>
    public virtual Order? Order { get; private set; }

    /// <summary>
    ///     Gets or sets the name of the product.
    /// </summary>
    public string ProductName { get; private set; }

    /// <summary>
    ///     Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    ///     Gets or sets the price per unit of the product.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    ///     Private parameterless constructor for EF Core and deserialization.
    ///     Initializes Product Name to empty string to avoid null warnings.
    /// </summary>
    private OrderItem()
    {
        ProductName = string.Empty;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="OrderItem" /> class.
    /// </summary>
    /// <param name="orderId">The ID of the parent order.</param>
    /// <param name="productName">The name of the product.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if quantity is not positive or unit price is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown if productName is null or whitespace.</exception>
    public OrderItem(Guid orderId, string productName, int quantity, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentNullException(nameof(productName));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        if (unitPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");

        OrderItemId = Guid.NewGuid();
        OrderId = orderId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}