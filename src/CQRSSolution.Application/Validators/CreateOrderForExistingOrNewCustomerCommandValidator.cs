using CQRSSolution.Application.Commands.CreateOrderForExistingOrNewCustomer;
using FluentValidation;

namespace CQRSSolution.Application.Validators;

public class CreateOrderForExistingOrNewCustomerCommandValidator : AbstractValidator<CreateOrderForExistingOrNewCustomerCommand>
{
    public CreateOrderForExistingOrNewCustomerCommandValidator()
    {
        RuleFor(v => v.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.")
            .MaximumLength(200).WithMessage("Customer name must not exceed 200 characters.");

        RuleFor(v => v.CustomerEmail)
            .NotEmpty().WithMessage("Customer email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(254).WithMessage("Customer email must not exceed 254 characters.");

        RuleFor(v => v.Items)
            .NotEmpty().WithMessage("Order must contain at least one item.");

        RuleForEach(v => v.Items).SetValidator(new OrderItemDtoValidator());
    }
}
