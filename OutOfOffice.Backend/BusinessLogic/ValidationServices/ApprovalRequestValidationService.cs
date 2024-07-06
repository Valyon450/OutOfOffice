using BusinessLogic.Requests;
using BusinessLogic.ValidationServices.Interfaces;
using DataAccess.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.ValidationServices
{
    public class ApprovalRequestValidationService : IApprovalRequestValidationService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IValidator<CreateOrUpdateApprovalRequest> _validator;

        public ApprovalRequestValidationService(IOutOfOfficeDbContext context, IValidator<CreateOrUpdateApprovalRequest> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<ValidationResult> ValidateAsync(CreateOrUpdateApprovalRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if (!await ApproverIdExists(request.ApproverId))
            {
                validationResult.Errors.Add(new ValidationFailure(nameof(request.ApproverId), "ApproverId does not exist"));
            }

            if (!await LeaveRequestIdExists(request.LeaveRequestId))
            {
                validationResult.Errors.Add(new ValidationFailure(nameof(request.LeaveRequestId), "LeaveRequestId does not exist"));
            }

            return validationResult;
        }

        private async Task<bool> ApproverIdExists(int id)
        {
            return await _context.Employee.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> LeaveRequestIdExists(int id)
        {
            return await _context.LeaveRequest.AnyAsync(e => e.Id == id);
        }
    }
}
