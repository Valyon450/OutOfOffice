using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
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
        public async Task<IActionResult> AddOrUpdateEmployee([FromBody] EmployeeDTO employeeDTO, CancellationToken cancellationToken)
        {
            bool success = await _employeeService.AddOrUpdateEmployeeAsync(employeeDTO, cancellationToken);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("{id}/deactivate")]
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

        [HttpPost("filter")]
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
