using BusinessLogic.Requests;
using BusinessLogic.ValidationServices.Interfaces;
using DataAccess.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.ValidationServices
{
    public class ProjectValidationService : IProjectValidationService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IValidator<CreateOrUpdateProject> _validator;

        public ProjectValidationService(IOutOfOfficeDbContext context, IValidator<CreateOrUpdateProject> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<ValidationResult> ValidateAsync(CreateOrUpdateProject request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if (!await ProjectManagerIdExists(request.ProjectManagerId))
            {
                validationResult.Errors.Add(new ValidationFailure(nameof(request.ProjectManagerId), "ProjectManagerId must exist and have a position of 'Project Manager'"));
            }

            return validationResult;
        }

        private async Task<bool> ProjectManagerIdExists(int id)
        {
            return await _context.Employee.AnyAsync(e => e.Id == id && e.Position == "Project Manager");
        }
    }

}
