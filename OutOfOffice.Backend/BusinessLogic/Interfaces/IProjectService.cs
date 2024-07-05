using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;

namespace BusinessLogic.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDTO>?> GetProjectsAsync(CancellationToken cancellationToken);
        Task<ProjectDTO?> GetProjectByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateProjectAsync(CreateOrUpdateProject request, CancellationToken cancellationToken);
        Task<bool> UpdateProjectAsync(int id, CreateOrUpdateProject request, CancellationToken cancellationToken);
        Task<bool> DeactivateProjectAsync(int id, CancellationToken cancellationToken);
        Task<bool> DeleteProjectAsync(int id, CancellationToken cancellationToken);
        Task<List<ProjectDTO>?> SearchProjectsAsync(string searchTerm, CancellationToken cancellationToken);
        Task<List<ProjectDTO>?> FilterProjectsAsync(FilterOptions options, CancellationToken cancellationToken);
    }
}
