using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;
using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> DeactivateEmployee(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _employeeService.DeactivateEmployeeAsync(id, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
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
