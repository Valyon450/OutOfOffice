﻿using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DataAccess.Interfaces;
using BusinessLogic.DTOs;

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

        public async Task<bool> AddOrUpdateEmployeeAsync(EmployeeDTO employeeDTO, CancellationToken cancellationToken)
        {
            try
            {
                var employee = _mapper.Map<Employee>(employeeDTO);

                if (employeeDTO.Id == 0)
                {                    
                    _context.Employees.Add(employee);
                }                    
                else
                {
                    _context.Employees.Update(employee);
                }                    

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
