using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DataAccess.Interfaces;
using BusinessLogic.DTOs;

namespace BusinessLogic.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IMapper _mapper;

        public ProjectService(OutOfOfficeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProjectDTO>?> GetProjectsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var projects = await _context.Projects.ToListAsync(cancellationToken);
                return _mapper.Map<List<ProjectDTO>>(projects);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id, cancellationToken);
                return _mapper.Map<ProjectDTO>(project);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task<bool> AddOrUpdateProjectAsync(ProjectDTO projectDTO, CancellationToken cancellationToken)
        {
            try
            {
                var project = _mapper.Map<Project>(projectDTO);

                if (projectDTO.Id == 0)
                {
                    _context.Projects.Add(project);
                }
                else
                {
                    _context.Projects.Update(project);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                return false;
            }
        }

        public async Task<bool> DeactivateProjectAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    return false;
                }
                else
                {
                    project.Status = "Inactive"; // Assuming "Inactive" is a valid status
                    await _context.SaveChangesAsync(cancellationToken);

                    return true;
                }
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                return false;
            }
        }

        public async Task<List<ProjectDTO>?> SearchProjectsAsync(string searchTerm, CancellationToken cancellationToken)
        {
            try
            {
                var projects = await _context.Projects
                                            .Where(p => p.Id.ToString()
                                            .Contains(searchTerm))
                                            .ToListAsync(cancellationToken);

                return _mapper.Map<List<ProjectDTO>>(projects);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task<List<ProjectDTO>?> FilterProjectsAsync(FilterOptions options, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Projects.AsQueryable();

                if (options.Status != null)
                    query = query.Where(p => p.Status == options.Status);

                // TODO: Add more filters based on options

                var projects = await query.ToListAsync(cancellationToken);

                return _mapper.Map<List<ProjectDTO>>(projects);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
    }
}
