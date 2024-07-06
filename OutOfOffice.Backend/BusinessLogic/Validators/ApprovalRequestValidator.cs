using BusinessLogic.Requests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Validators
{
    public class ApprovalRequestValidator : AbstractValidator<CreateOrUpdateApprovalRequest>
    {
        public ApprovalRequestValidator()
        {
            RuleFor(x => x.ApproverId)
                .GreaterThan(0)
                .WithMessage("ApproverId must be a positive number");

            RuleFor(x => x.LeaveRequestId)
                .GreaterThan(0)
                .WithMessage("LeaveRequestId must be a positive number");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required")
                .Must(status => new[] { "New", "Approved", "Rejected" }.Contains(status))
                .WithMessage("Status must be either 'New', 'Approved' or 'Rejected'");

            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                .WithMessage("Comment cannot be longer than 1000 characters");
        }
    }
}
