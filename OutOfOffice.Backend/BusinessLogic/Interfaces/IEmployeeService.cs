using BusinessLogic.Services;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task AddOrUpdateEmployeeAsync(Employee employee);
        Task DeactivateEmployeeAsync(int id);
        Task<List<Employee>> SearchEmployeesAsync(string searchTerm);
        Task<List<Employee>> FilterEmployeesAsync(FilterOptions options);
    }
}
