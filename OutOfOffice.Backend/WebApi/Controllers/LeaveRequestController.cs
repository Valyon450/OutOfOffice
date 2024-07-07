using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;
using BusinessLogic.Services.Interfaces;
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
            try
            {
                var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id, cancellationToken);

                return Ok(leaveRequest);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateOrUpdateLeaveRequest request, CancellationToken cancellationToken)
        {
            try
            {
                int id = await _leaveRequestService.CreateLeaveRequestAsync(request, cancellationToken);

                return CreatedAtAction(nameof(GetLeaveRequestById), new { id }, request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeaveRequest(int id, [FromBody] CreateOrUpdateLeaveRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _leaveRequestService.UpdateLeaveRequestAsync(id, request, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> SubmitOrCancelLeaveRequest(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _leaveRequestService.SubmitOrCancelLeaveRequestAsync(id, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _leaveRequestService.DeleteLeaveRequestAsync(id, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
