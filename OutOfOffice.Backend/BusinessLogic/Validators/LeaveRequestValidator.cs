using BusinessLogic.Requests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Validators
{
    public class LeaveRequestValidator : AbstractValidator<CreateOrUpdateLeaveRequest>
    {
        public LeaveRequestValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty()
                .WithMessage("EmployeeId is required")
                .GreaterThan(0)
                .WithMessage("EmployeeId must be a positive number");

            RuleFor(x => x.AbsenceReason)
                .NotEmpty()
                .WithMessage("AbsenceReason is required")
                .MaximumLength(255)
                .WithMessage("AbsenceReason cannot be longer than 255 characters");

            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                .WithMessage("Comment cannot be longer than 255 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("StartDate is required")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("StartDate must be in the future")
                .LessThan(x => x.EndDate)
                .WithMessage("StartDate must be earlier than EndDate");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("EndDate is required")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("EndDate must be in the future");

            RuleFor(x => x.Status)
                .Must(status => new[] { "New", "Submitted", "Canceled", "Approved", "Rejected" }.Contains(status))
                .WithMessage("Status must be either 'New', 'Submitted', 'Canceled', 'Approved' or 'Rejected'");
        }
    }
}
