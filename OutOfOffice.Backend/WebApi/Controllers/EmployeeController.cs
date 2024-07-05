using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Options;
using BusinessLogic.Requests;
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
            var employee = await _employeeService.GetEmployeeByIdAsync(id, cancellationToken);

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(employee);
            }            
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateOrUpdateEmployee request, CancellationToken cancellationToken)
        {
            int id = await _employeeService.CreateEmployeeAsync(request, cancellationToken);

            if (id != 0)
            {
                return CreatedAtAction(nameof(GetEmployeeById), new { id }, request);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CreateOrUpdateEmployee request, CancellationToken cancellationToken)
        {
            bool success = await _employeeService.UpdateEmployeeAsync(id, request, cancellationToken);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> DeactivateEmployee(int id, CancellationToken cancellationToken)
        {
            bool success = await _employeeService.DeactivateEmployeeAsync(id, cancellationToken);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id, CancellationToken cancellationToken)
        {
            bool success = await _employeeService.DeleteEmployeeAsync(id, cancellationToken);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
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
