using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Options;
using BusinessLogic.Requests;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaveRequestDTO>>> GetLeaveRequests(CancellationToken cancellationToken)
        {
            var leaveRequests = await _leaveRequestService.GetLeaveRequestsAsync(cancellationToken);

            if (leaveRequests == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(leaveRequests);
            }            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDTO>> GetLeaveRequestById(int id, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id, cancellationToken);

            if (leaveRequest == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(leaveRequest);
            }            
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateOrUpdateLeaveRequest request, CancellationToken cancellationToken)
        {
            int id = await _leaveRequestService.CreateLeaveRequestAsync(request, cancellationToken);

            if (id != 0)
            {
                return CreatedAtAction(nameof(GetLeaveRequestById), new { id }, request);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeaveRequest(int id, [FromBody] CreateOrUpdateLeaveRequest request, CancellationToken cancellationToken)
        {
            bool success = await _leaveRequestService.UpdateLeaveRequestAsync(id, request, cancellationToken);

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
        public async Task<IActionResult> CancelLeaveRequest(int id, CancellationToken cancellationToken)
        {
            bool success = await _leaveRequestService.CancelLeaveRequestAsync(id, cancellationToken);

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
        public async Task<IActionResult> DeleteLeaveRequest(int id, CancellationToken cancellationToken)
        {
            bool success = await _leaveRequestService.DeleteLeaveRequestAsync(id, cancellationToken);

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
        public async Task<ActionResult<List<LeaveRequestDTO>>> SearchLeaveRequests([FromQuery] string searchTerm, CancellationToken cancellationToken)
        {
            var leaveRequests = await _leaveRequestService.SearchLeaveRequestsAsync(searchTerm, cancellationToken);

            if (leaveRequests == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(leaveRequests);
            }            
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<LeaveRequestDTO>>> FilterLeaveRequests([FromBody] FilterOptions options, CancellationToken cancellationToken)
        {
            var leaveRequests = await _leaveRequestService.FilterLeaveRequestsAsync(options, cancellationToken);

            if (leaveRequests == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(leaveRequests);
            }            
        }
    }
}
