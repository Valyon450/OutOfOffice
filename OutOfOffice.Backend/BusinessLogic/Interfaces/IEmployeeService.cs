using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;

namespace BusinessLogic.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDTO>?> GetEmployeesAsync(CancellationToken cancellationToken);
        Task<EmployeeDTO?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateEmployeeAsync(CreateOrUpdateEmployee request, CancellationToken cancellationToken);
        Task<bool> UpdateEmployeeAsync(int id, CreateOrUpdateEmployee request, CancellationToken cancellationToken);
        Task<bool> DeactivateEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<List<EmployeeDTO>?> SearchEmployeesAsync(string searchTerm, CancellationToken cancellationToken);
        Task<List<EmployeeDTO>?> FilterEmployeesAsync(FilterOptions options, CancellationToken cancellationToken);
    }
}
