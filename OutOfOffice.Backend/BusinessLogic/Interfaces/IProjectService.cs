using BusinessLogic.Services;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task AddOrUpdateProjectAsync(Project project);
        Task DeactivateProjectAsync(int id);
        Task<List<Project>> SearchProjectsAsync(string searchTerm);
        Task<List<Project>> FilterProjectsAsync(FilterOptions options);
    }
}
