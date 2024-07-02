using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class ProjectService : IProjectService
    {
        private readonly OutOfOfficeDbContext _context;

        public ProjectService(OutOfOfficeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task AddOrUpdateProjectAsync(Project project)
        {
            if (project.ID == 0)
                _context.Projects.Add(project);
            else
                _context.Projects.Update(project);

            await _context.SaveChangesAsync();
        }

        public async Task DeactivateProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.Status = "Inactive"; // Assuming "Inactive" is a valid status
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Project>> SearchProjectsAsync(string searchTerm)
        {
            var projects = await _context.Projects
                .Where(p => p.ID.ToString().Contains(searchTerm))
                .ToListAsync();

            return projects;
        }

        public async Task<List<Project>> FilterProjectsAsync(FilterOptions options)
        {
            var query = _context.Projects.AsQueryable();

            if (options.Status != null)
                query = query.Where(p => p.Status == options.Status);

            // Add more filters based on options as needed

            return await query.ToListAsync();
        }
    }
}
