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
    public class ApprovalRequestController : ControllerBase
    {
        private readonly IApprovalRequestService _approvalRequestService;

        public ApprovalRequestController(IApprovalRequestService approvalRequestService)
        {
            _approvalRequestService = approvalRequestService;
        }

        /// <summary>
        /// Retrieves all approval requests.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of approval requests.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ApprovalRequestDTO>>> GetApprovalRequests(CancellationToken cancellationToken)
        {
            var approvalRequests = await _approvalRequestService.GetApprovalRequestsAsync(cancellationToken);

            if (approvalRequests == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(approvalRequests);
            }            
        }

        /// <summary>
        /// Retrieves a specific approval request.
        /// </summary>
        /// <param name="id">Approval request Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>The approval request.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApprovalRequestDTO>> GetApprovalRequestById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var approvalRequest = await _approvalRequestService.GetApprovalRequestByIdAsync(id, cancellationToken);

                return Ok(approvalRequest);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new approval request.
        /// </summary>
        /// <param name="request">Approval request data.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>Created approval request Id.</returns>
        /// <response code="201">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPost]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateApprovalRequest([FromBody] CreateOrUpdateApprovalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                int id = await _approvalRequestService.CreateApprovalRequestAsync(request, cancellationToken);

                return CreatedAtAction(nameof(GetApprovalRequestById), new { id }, request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing approval request.
        /// </summary>
        /// <param name="id">Approval request Id.</param>
        /// <param name="request">Updated approval request data.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")] // Only administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateApprovalRequest(int id, [FromBody] CreateOrUpdateApprovalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _approvalRequestService.UpdateApprovalRequestAsync(id, request, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Approves an approval request.
        /// </summary>
        /// <param name="id">Approval request Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPatch("{id}/approve")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApproveRequest(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _approvalRequestService.ApproveRequestAsync(id, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Rejects an approval request.
        /// </summary>
        /// <param name="id">Approval request Id.</param>
        /// <param name="rejectionReason">Reason for rejection.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPatch("{id}/reject")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RejectRequest(int id, [FromBody] string rejectionReason, CancellationToken cancellationToken)
        {
            try
            {
                await _approvalRequestService.RejectRequestAsync(id, rejectionReason, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an approval request.
        /// </summary>
        /// <param name="id">Approval request Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // Only administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteApprovalRequest(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _approvalRequestService.DeleteApprovalRequestAsync(id, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Searches approval requests based on a search term.
        /// </summary>
        /// <param name="searchTerm">Term to search for.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of approval requests matching the search term.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("search")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ApprovalRequestDTO>>> SearchApprovalRequests([FromQuery] string searchTerm, CancellationToken cancellationToken)
        {
            var approvalRequests = await _approvalRequestService.SearchApprovalRequestsAsync(searchTerm, cancellationToken);

            if (approvalRequests == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(approvalRequests);
            }           
        }

        /// <summary>
        /// Filters approval requests based on filter options.
        /// </summary>
        /// <param name="options">Filter options.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of approval requests matching the filter options.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpPost("filter")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ApprovalRequestDTO>>> FilterApprovalRequests([FromBody] FilterOptions options, CancellationToken cancellationToken)
        {
            var approvalRequests = await _approvalRequestService.FilterApprovalRequestsAsync(options, cancellationToken);

            if (approvalRequests == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(approvalRequests);
            }            
        }    
    }
}
