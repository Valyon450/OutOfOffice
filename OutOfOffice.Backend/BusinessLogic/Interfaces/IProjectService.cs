﻿using BusinessLogic.Services;
using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDTO>?> GetProjectsAsync(CancellationToken cancellationToken);
        Task<ProjectDTO?> GetProjectByIdAsync(int id, CancellationToken cancellationToken);
        Task AddOrUpdateProjectAsync(ProjectDTO project, CancellationToken cancellationToken);
        Task DeactivateProjectAsync(int id, CancellationToken cancellationToken);
        Task<List<ProjectDTO>?> SearchProjectsAsync(string searchTerm, CancellationToken cancellationToken);
        Task<List<ProjectDTO>?> FilterProjectsAsync(FilterOptions options, CancellationToken cancellationToken);
    }
}
