using BusinessLogic.Requests;
using BusinessLogic.ValidationServices.Interfaces;
using DataAccess.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.ValidationServices
{
    public class LeaveRequestValidationService : ILeaveRequestValidationService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IValidator<CreateOrUpdateLeaveRequest> _validator;

        public LeaveRequestValidationService(IOutOfOfficeDbContext context, IValidator<CreateOrUpdateLeaveRequest> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<ValidationResult> ValidateAsync(CreateOrUpdateLeaveRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if (!await EmployeeIdExists(request.EmployeeId))
            {
                validationResult.Errors.Add(new ValidationFailure(nameof(request.EmployeeId), "EmployeeId does not exist"));
            }

            return validationResult;
        }

        private async Task<bool> EmployeeIdExists(int id)
        {
            return await _context.Employee.AnyAsync(e => e.Id == id);
        }
    }

}
