using BusinessLogic.Requests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Validators
{
    public class EmployeeValidator : AbstractValidator<CreateOrUpdateEmployee>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("FullName is required")
                .MaximumLength(255)
                .WithMessage("FullName cannot be longer than 255 characters");

            RuleFor(x => x.Subdivision)
                .NotEmpty()
                .WithMessage("Subdivision is required")
                .MaximumLength(255)
                .WithMessage("Subdivision cannot be longer than 255 characters");

            RuleFor(x => x.Position)
                .NotEmpty()
                .WithMessage("Position is required")
                .MaximumLength(255)
                .WithMessage("Position cannot be longer than 255 characters");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required")
                .Must(status => new[] { "Active", "Inactive" }.Contains(status))
                .WithMessage("Status must be either 'Active' or 'Inactive'");

            RuleFor(x => x.PeoplePartnerId)
                .NotEmpty()
                .WithMessage("PeoplePartnerId is required")
                .GreaterThan(0)
                .WithMessage("PeoplePartnerId must be a positive number");

            RuleFor(x => x.OutOfOfficeBalance)
                .NotEmpty()
                .WithMessage("OutOfOfficeBalance is required")
                .GreaterThanOrEqualTo(0)
                .WithMessage("OutOfOfficeBalance must be a non-negative number");
        }
    }
}
