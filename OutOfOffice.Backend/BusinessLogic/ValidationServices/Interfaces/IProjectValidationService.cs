using BusinessLogic.Requests;
using FluentValidation.Results;

namespace BusinessLogic.ValidationServices.Interfaces
{
    public interface IProjectValidationService
    {
        Task<ValidationResult> ValidateAsync(CreateOrUpdateProject request);
    }
}
