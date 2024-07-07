using BusinessLogic.Requests;
using BusinessLogic.ValidationServices.Interfaces;
using DataAccess.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.ValidationServices
{
    public class EmployeeValidationService : IEmployeeValidationService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IValidator<CreateOrUpdateEmployee> _validator;

        public EmployeeValidationService(IOutOfOfficeDbContext context, IValidator<CreateOrUpdateEmployee> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<ValidationResult> ValidateAsync(CreateOrUpdateEmployee request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if (!await PeoplePartnerIsHrManager(request.PeoplePartnerId))
            {
                validationResult.Errors.Add(new ValidationFailure(nameof(request.PeoplePartnerId), "PeoplePartner must exist and have a position of 'HR Manager'"));
            }

            if (!await ProjectIdExists(request.ProjectId))
            {
                validationResult.Errors.Add(new ValidationFailure(nameof(request.ProjectId), "ProjectId does not exist"));
            }

            return validationResult;
        }

        private async Task<bool> PeoplePartnerIsHrManager(int id)
        {
            return await _context.Employee.AnyAsync(e => e.Id == id && e.Position == "HR Manager");
        }

        private async Task<bool> ProjectIdExists(int id)
        {
            return await _context.Project.AnyAsync(p => p.Id == id);
        }
    }
}
