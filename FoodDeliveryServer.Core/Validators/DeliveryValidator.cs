using FluentValidation;
using FoodDeliveryServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryServer.Core.Validators
{
    public class DeliveryValidator : AbstractValidator<Delivery>
    {
        public DeliveryValidator()
        {
            Include(new UserValidator());

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required")
                .MaximumLength(20).WithMessage("Phone is too long");

            RuleFor(x => x.Vehical)
                .NotEmpty().WithMessage("Vehical details is required")
                .MaximumLength(50).WithMessage("Vehical details is too long");
        }
    }
}
