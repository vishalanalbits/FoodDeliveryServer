using FluentValidation;
using FoodDeliveryServer.Data.Models;

namespace FoodDeliveryServer.Core.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.RestaurantId)
                .NotEmpty().WithMessage("Restaurant Id is required");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("The 'Items' list shouldn't be empty")
                .ForEach(x => x.SetValidator(new OrderItemValidator()));

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(200).WithMessage("Address is too long");

            RuleFor(x => x.Coordinate)
                .NotNull().WithMessage("Coordinate is required");

            RuleFor(x => x.PaymentIntentId)
                .NotNull().WithMessage("Payment intent is required")
                .MaximumLength(255).WithMessage("Payment intent id is too long");
        }
    }

    public class OrderItemValidator : AbstractValidator<OrderItem>
    {
        public OrderItemValidator()
        {
            RuleFor(x => x.MenuId)
                .NotEmpty().WithMessage("Menu Id is required");

            RuleFor(x => x.Quantity)
                .NotNull().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero");
        }
    }
}
