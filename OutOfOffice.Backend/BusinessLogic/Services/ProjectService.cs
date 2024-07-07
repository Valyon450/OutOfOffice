using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DataAccess.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.ValidationServices.Interfaces;

namespace BusinessLogic.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProjectValidationService _projectValidationService;
        private readonly ILogger<IProjectService> _logger;

        public ProjectService(IOutOfOfficeDbContext context, IMapper mapper, IProjectValidationService projectValidationService, ILogger<ProjectService> logger)
        {
            _context = context;
            _mapper = mapper;
            _projectValidationService = projectValidationService;
            _logger = logger;
        }

        public async Task<List<ProjectDTO>?> GetProjectsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var projects = await _context.Project.ToListAsync(cancellationToken);
                return _mapper.Map<List<ProjectDTO>>(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting projects.");
                return null;
            }
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _context.Project.FindAsync(new object[] { id }, cancellationToken);

                if (project == null)
                {
                    throw new Exception($"Project with Id: {id} not found.");
                }

                return _mapper.Map<ProjectDTO>(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting project by Id: {id}.");
                throw;
            }
        }

        public async Task<int> CreateProjectAsync(CreateOrUpdateProject request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _projectValidationService.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var project = _mapper.Map<Project>(request);

                _context.Project.Add(project);

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Project with Id: {project.Id} has been created successfully.");

                return project.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a project.");
                throw;
            }
        }

        public async Task UpdateProjectAsync(int id, CreateOrUpdateProject request, CancellationToken cancellationToken)
        {
            try
            {
                var existingProject = await _context.Project.FindAsync(new object[] { id }, cancellationToken);

                if (existingProject == null)
                {
                    throw new Exception($"Project with Id: {id} not found.");
                }

                var validationResult = await _projectValidationService.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                // Map the updated properties from the request to the existing project
                _mapper.Map(request, existingProject);

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Project with Id: {id} has been updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating project with Id: {id}.");
                throw;
            }
        }        

        public async Task ActivateOrDeactivateProjectAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _context.Project.FindAsync(new object[] { id }, cancellationToken);

                if (project == null)
                {
                    throw new Exception($"Project with Id: {id} not found.");
                }

                if (project.Status == "Active")
                {
                    project.Status = "Inactive";
                }
                else
                {
                    project.Status = "Active";
                }

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Project with Id: {id} get activation change successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deactivating project with Id: {id}.");
                throw;
            }
        }

        public async Task DeleteProjectAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _context.Project.FindAsync(new object[] { id }, cancellationToken);

                if (project == null)
                {
                    throw new Exception($"Project with Id: {id} not found.");
                }
                
                _context.Project.Remove(project);

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Project with Id: {id} has been deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting project with Id: {id}");
                throw;
            }
        }

        public async Task<List<ProjectDTO>?> SearchProjectsAsync(string searchTerm, CancellationToken cancellationToken)
        {
            try
            {
                var projects = await _context.Project
                    .Where(p => p.Id.ToString().Contains(searchTerm))
                    .ToListAsync(cancellationToken);

                return _mapper.Map<List<ProjectDTO>>(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while searching projects with term: {searchTerm}.");
                return null;
            }
        }

        public async Task<List<ProjectDTO>?> FilterProjectsAsync(FilterOptions options, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Project.AsQueryable();

                if (options.Status != null)
                    query = query.Where(p => p.Status == options.Status);

                // TODO: Add more filters based on options

                var projects = await query.ToListAsync(cancellationToken);

                return _mapper.Map<List<ProjectDTO>>(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while filtering projects.");
                return null;
            }
        }
    }
}
