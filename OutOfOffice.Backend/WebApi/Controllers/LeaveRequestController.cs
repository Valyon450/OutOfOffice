using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
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
        public async Task<IActionResult> AddOrUpdateLeaveRequest([FromBody] LeaveRequestDTO leaveRequestDTO, CancellationToken cancellationToken)
        {
            bool success = await _leaveRequestService.AddOrUpdateLeaveRequestAsync(leaveRequestDTO, cancellationToken);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("{id}/cancel")]
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

        [HttpPost("filter")]
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
