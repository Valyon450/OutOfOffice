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
    public class ApprovalRequestController : ControllerBase
    {
        private readonly IApprovalRequestService _approvalRequestService;

        public ApprovalRequestController(IApprovalRequestService approvalRequestService)
        {
            _approvalRequestService = approvalRequestService;
        }

        [HttpGet]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
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

        [HttpGet("{id}")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
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

        [HttpPost]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")] // Administrators can access this
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

        [HttpPatch("{id}/approve")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
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

        [HttpPatch("{id}/reject")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // Administrators can access this
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

        [HttpGet("search")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
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

        [HttpGet("filter")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
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
