﻿using BusinessLogic.Services;
using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDTO>?> GetEmployeesAsync(CancellationToken cancellationToken);
        Task<EmployeeDTO?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
        Task AddOrUpdateEmployeeAsync(EmployeeDTO employee, CancellationToken cancellationToken);
        Task DeactivateEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<List<EmployeeDTO>?> SearchEmployeesAsync(string searchTerm, CancellationToken cancellationToken);
        Task<List<EmployeeDTO>?> FilterEmployeesAsync(FilterOptions options, CancellationToken cancellationToken);
    }
}
