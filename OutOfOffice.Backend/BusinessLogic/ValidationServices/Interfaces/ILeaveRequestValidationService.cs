using BusinessLogic.Requests;
using FluentValidation.Results;

namespace BusinessLogic.ValidationServices.Interfaces
{
    public interface ILeaveRequestValidationService
    {
        Task<ValidationResult> ValidateAsync(CreateOrUpdateLeaveRequest request);
    }
}
