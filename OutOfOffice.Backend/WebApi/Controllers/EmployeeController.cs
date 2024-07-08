using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;
using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all actions in this controller
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        public async Task<ActionResult<List<EmployeeDTO>>> GetEmployees(CancellationToken cancellationToken)
        {
            var employees = await _employeeService.GetEmployeesAsync(cancellationToken);

            if (employees == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(employees);
            }            
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id, cancellationToken);

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "HR Manager, Administrator")] // HR Managers and Administrators can access this
        public async Task<IActionResult> CreateEmployee([FromBody] CreateOrUpdateEmployee request, CancellationToken cancellationToken)
        {
            try
            {
                int id = await _employeeService.CreateEmployeeAsync(request, cancellationToken);

                return CreatedAtAction(nameof(GetEmployeeById), new { id }, request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "HR Manager, Administrator")] // HR Managers and Administrators can access this
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CreateOrUpdateEmployee request, CancellationToken cancellationToken)
        {
            try
            {
                await _employeeService.UpdateEmployeeAsync(id, request, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/activation")]
        [Authorize(Roles = "HR Manager, Administrator")] // HR Managers and Administrators can access this
        public async Task<IActionResult> ActivateOrDeactivateEmployee(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _employeeService.ActivateOrDeactivateEmployeeAsync(id, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/project")]
        [Authorize(Roles = "Project Manager, Administrator")] // Project Managers and Administrators can access this
        public async Task<IActionResult> AssignEmployeeToProject(int id, int projectId, CancellationToken cancellationToken)
        {
            try
            {
                await _employeeService.AssignEmployeeToProjectAsync(id, projectId, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // Administrators can access this
        public async Task<IActionResult> DeleteEmployee(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(id, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        public async Task<ActionResult<List<EmployeeDTO>>> SearchEmployees([FromQuery] string searchTerm, CancellationToken cancellationToken)
        {
            var employees = await _employeeService.SearchEmployeesAsync(searchTerm, cancellationToken);

            if (employees == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(employees);
            }            
        }

        [HttpGet("filter")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        public async Task<ActionResult<List<EmployeeDTO>>> FilterEmployees([FromBody] FilterOptions options, CancellationToken cancellationToken)
        {
            var employees = await _employeeService.FilterEmployeesAsync(options, cancellationToken);

            if (employees == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(employees);
            }            
        }
    }
}
