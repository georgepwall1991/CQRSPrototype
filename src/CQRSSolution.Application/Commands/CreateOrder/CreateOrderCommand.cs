using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CQRSSolution.Application.DTOs; // For CreateOrderItemDto

namespace CQRSSolution.Application.Commands.CreateOrder;

/// <summary>
/// Command to create a new order.
/// Returns the ID of the newly created order.
/// </summary>
public class CreateOrderCommand : IRequest<Guid>
{
    /// <summary>
    /// Gets or sets the name of the customer placing the order.
    /// </summary>
    [Required(ErrorMessage = "Customer name is required.")]
    [MaxLength(200, ErrorMessage = "Customer name cannot exceed 200 characters.")]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email of the customer.
    /// </summary>
    [Required(ErrorMessage = "Customer email is required.")]
    [MaxLength(254, ErrorMessage = "Customer email cannot exceed 254 characters.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of items to be included in the order.
    /// Must contain at least one item.
    /// </summary>
    [Required(ErrorMessage = "Order items are required.")]
    [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
    public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
}