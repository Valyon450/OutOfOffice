using BusinessLogic.Requests;
using FluentValidation.Results;

namespace BusinessLogic.ValidationServices.Interfaces
{
    public interface IEmployeeValidationService
    {
        Task<ValidationResult> ValidateAsync(CreateOrUpdateEmployee request);
    }
}
