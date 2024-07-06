using BusinessLogic.Requests;
using DataAccess.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Validators
{
    public class ProjectValidator : AbstractValidator<CreateOrUpdateProject>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.ProjectType)
                .NotEmpty()
                .WithMessage("ProjectType is required")
                .MaximumLength(255)
                .WithMessage("ProjectType cannot be longer than 255 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("StartDate is required")
                .LessThan(x => x.EndDate)
                .WithMessage("StartDate must be earlier than EndDate");

            RuleFor(x => x.ProjectManagerId)
                .NotEmpty()
                .WithMessage("ProjectManagerId is required")
                .GreaterThan(0)
                .WithMessage("ProjectManagerId must be a positive number");

            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                .WithMessage("Comment cannot be longer than 1000 characters");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required")
                .Must(status => new[] { "Active", "Inactive" }.Contains(status))
                .WithMessage("Status must be either 'Active' or 'Inactive'");
        }
    }
}
