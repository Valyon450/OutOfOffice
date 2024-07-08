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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of employees.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Retrieves a specific employee.
        /// </summary>
        /// <param name="id">Employee Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>The employee.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="request">Employee data.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>Created employee Id.</returns>
        /// <response code="201">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPost]
        [Authorize(Roles = "HR Manager, Administrator")] // HR Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">Employee Id.</param>
        /// <param name="request">Updated employee data.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "HR Manager, Administrator")] // HR Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Activates or deactivates an employee.
        /// </summary>
        /// <param name="id">Employee Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPatch("{id}/activation")]
        [Authorize(Roles = "HR Manager, Administrator")] // HR Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Assigns an employee to a project.
        /// </summary>
        /// <param name="id">Employee Id.</param>
        /// <param name="projectId">Project Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPatch("{id}/project")]
        [Authorize(Roles = "Project Manager, Administrator")] // Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Deletes an employee.
        /// </summary>
        /// <param name="id">Employee Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // Only administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Searches employees based on a search term.
        /// </summary>
        /// <param name="searchTerm">Term to search for.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of employees matching the search term.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("search")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Filters employees based on filter options.
        /// </summary>
        /// <param name="options">Filter options.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of employees matching the filter options.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpPost("filter")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
