using BusinessLogic.Requests;
using FluentValidation.Results;

namespace BusinessLogic.ValidationServices.Interfaces
{
    public interface IApprovalRequestValidationService
    {
        Task<ValidationResult> ValidateAsync(CreateOrUpdateApprovalRequest request);
    }
}
