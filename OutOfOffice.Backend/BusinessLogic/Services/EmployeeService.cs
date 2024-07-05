using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DataAccess.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;

namespace BusinessLogic.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(OutOfOfficeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDTO>?> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var employees = await _context.Employees.ToListAsync(cancellationToken);
                return _mapper.Map<List<EmployeeDTO>>(employees);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task<EmployeeDTO?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id, cancellationToken);
                return _mapper.Map<EmployeeDTO>(employee);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task<int> CreateEmployeeAsync(CreateOrUpdateEmployee request, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: Validation

                var employee = _mapper.Map<Employee>(request);

                _context.Employees.Add(employee);

                await _context.SaveChangesAsync(cancellationToken);

                return employee.Id;
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                return 0;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(int id, CreateOrUpdateEmployee request, CancellationToken cancellationToken)
        {
            try
            {
                var existingEmployee = await _context.Employees.FindAsync(id);

                if (existingEmployee == null)
                {
                    return false; // Approval request not found
                }

                // Map the updated properties from the request to the existing approval request
                _mapper.Map(request, existingEmployee);

                // TODO: Validation

                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                return false;
            }
        }

        public async Task<bool> DeactivateEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return false;
                }
                else
                {
                    employee.Status = "Inactive"; // Assuming "Inactive" is a valid status
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

        public async Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                {
                    return false; // Approval request not found
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                return false;
            }
        }

        public async Task<List<EmployeeDTO>?> SearchEmployeesAsync(string searchTerm, CancellationToken cancellationToken)
        {
            try
            {
                var employees = await _context.Employees
                                        .Where(e => e.FullName.Contains(searchTerm))
                                        .ToListAsync(cancellationToken);

                return _mapper.Map<List<EmployeeDTO>>(employees);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task<List<EmployeeDTO>?> FilterEmployeesAsync(FilterOptions options, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Employees.AsQueryable();

                if (options.IsActive.HasValue)
                    query = query.Where(e => e.Status == (options.IsActive.Value ? "Active" : "Inactive"));

                // TODO: Add more filters based on options

                var employees = await query.ToListAsync(cancellationToken);

                return _mapper.Map<List<EmployeeDTO>>(employees);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
    }
}
