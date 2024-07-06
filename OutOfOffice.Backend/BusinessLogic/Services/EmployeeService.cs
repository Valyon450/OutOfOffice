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
    public class EmployeeService : IEmployeeService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmployeeValidationService _employeeValidationService;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IOutOfOfficeDbContext context, IMapper mapper, IEmployeeValidationService employeeValidationService, ILogger<EmployeeService> logger)
        {
            _context = context;
            _mapper = mapper;
            _employeeValidationService = employeeValidationService;
            _logger = logger;
        }

        public async Task<List<EmployeeDTO>?> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var employees = await _context.Employee.ToListAsync(cancellationToken);
                return _mapper.Map<List<EmployeeDTO>>(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employees");
                return null;
            }
        }

        public async Task<EmployeeDTO?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _context.Employee.FindAsync(new object[] { id }, cancellationToken);

                if (employee == null)
                {
                    throw new Exception($"Employee with Id: {id} not found.");
                }

                return _mapper.Map<EmployeeDTO>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching employee with Id: {id}");
                throw;
            }
        }

        public async Task<int> CreateEmployeeAsync(CreateOrUpdateEmployee request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _employeeValidationService.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var employee = _mapper.Map<Employee>(request);

                _context.Employee.Add(employee);

                await _context.SaveChangesAsync(cancellationToken);

                return employee.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an employee");
                throw;
            }
        }

        public async Task UpdateEmployeeAsync(int id, CreateOrUpdateEmployee request, CancellationToken cancellationToken)
        {
            try
            {
                var existingEmployee = await _context.Employee.FindAsync(new object[] { id }, cancellationToken);

                if (existingEmployee == null)
                {
                    throw new Exception($"Employee with Id: {id} not found.");
                }

                var validationResult = await _employeeValidationService.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                // Map the updated properties from the request to the existing employee
                _mapper.Map(request, existingEmployee);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating employee with Id: {id}");
                throw;
            }
        }

        public async Task DeactivateEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _context.Employee.FindAsync(new object[] { id }, cancellationToken);

                if (employee == null)
                {
                    throw new Exception($"Employee with Id: {id} not found.");
                }

                employee.Status = "Inactive"; // Assuming "Inactive" is a valid status
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deactivating employee with Id: {id}");
                throw;
            }
        }

        public async Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _context.Employee.FindAsync(new object[] { id }, cancellationToken);

                if (employee == null)
                {
                    throw new Exception($"Employee with Id: {id} not found.");
                }

                _context.Employee.Remove(employee);
                await _context.SaveChangesAsync(cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting employee with Id: {id}");
                throw;
            }
        }

        public async Task<List<EmployeeDTO>?> SearchEmployeesAsync(string searchTerm, CancellationToken cancellationToken)
        {
            try
            {
                var employees = await _context.Employee
                    .Where(e => e.FullName.Contains(searchTerm))
                    .ToListAsync(cancellationToken);

                return _mapper.Map<List<EmployeeDTO>>(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for employees");
                return null;
            }
        }

        public async Task<List<EmployeeDTO>?> FilterEmployeesAsync(FilterOptions options, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Employee.AsQueryable();

                if (options.Status != null)
                    query = query.Where(e => e.Status == options.Status);

                //TODO Add more filters based on options

                var employees = await query.ToListAsync(cancellationToken);

                return _mapper.Map<List<EmployeeDTO>>(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while filtering employees");
                return null;
            }
        }
    }
}
