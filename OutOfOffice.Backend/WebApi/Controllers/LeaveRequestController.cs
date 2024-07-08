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
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        /// <summary>
        /// Retrieves all leave requests.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of leave requests.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Retrieves a specific leave request.
        /// </summary>
        /// <param name="id">Leave request Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>The leave request.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Creates a new leave request.
        /// </summary>
        /// <param name="request">Leave request data.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>Created leave request Id.</returns>
        /// <response code="201">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Updates an existing leave request.
        /// </summary>
        /// <param name="id">Leave request Id.</param>
        /// <param name="request">Updated leave request data.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Submits or cancels a leave request.
        /// </summary>
        /// <param name="id">Leave request Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Deletes a leave request.
        /// </summary>
        /// <param name="id">Leave request Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // Administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Searches leave requests based on a search term.
        /// </summary>
        /// <param name="searchTerm">Term to search for.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of leave requests matching the search term.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("search")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Filters leave requests based on filter options.
        /// </summary>
        /// <param name="options">Filter options.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of leave requests matching the filter options.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpPost("filter")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
