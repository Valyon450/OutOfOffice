using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly OutOfOfficeDbContext _context;

        public EmployeeService(OutOfOfficeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task AddOrUpdateEmployeeAsync(Employee employee)
        {
            if (employee.ID == 0)
                _context.Employees.Add(employee);
            else
                _context.Employees.Update(employee);

            await _context.SaveChangesAsync();
        }

        public async Task DeactivateEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.Status = "Inactive"; // Assuming "Inactive" is a valid status
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            var employees = await _context.Employees
                .Where(e => e.FullName.Contains(searchTerm))
                .ToListAsync();

            return employees;
        }

        public async Task<List<Employee>> FilterEmployeesAsync(FilterOptions options)
        {
            var query = _context.Employees.AsQueryable();

            if (options.IsActive.HasValue)
                query = query.Where(e => e.Status == (options.IsActive.Value ? "Active" : "Inactive"));

            // Add more filters based on options as needed

            return await query.ToListAsync();
        }
    }
}
