using FluentValidation;
using FluentValidation.Validators;
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
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(6).WithMessage("Username is too short")
                .MaximumLength(20).WithMessage("Username is too long");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password is too short")
                .MaximumLength(128).WithMessage("Password is too long");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Email is not valid");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name is too long");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name is too long");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required")
                .MaximumLength(20).WithMessage("Phone is too long");

            RuleFor(x => x.Vehical)
                .NotEmpty().WithMessage("Vehical details is required")
                .MaximumLength(50).WithMessage("Vehical details is too long");
        }
    }
}
