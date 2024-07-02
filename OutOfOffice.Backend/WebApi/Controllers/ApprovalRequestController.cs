using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalRequestController : ControllerBase
    {
        private readonly IApprovalRequestService _approvalRequestService;

        public ApprovalRequestController(IApprovalRequestService approvalRequestService)
        {
            _approvalRequestService = approvalRequestService;
        }

        [HttpGet]
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
        public async Task<ActionResult<ApprovalRequestDTO>> GetApprovalRequestById(int id, CancellationToken cancellationToken)
        {
            var approvalRequest = await _approvalRequestService.GetApprovalRequestByIdAsync(id, cancellationToken);

            if (approvalRequest == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(approvalRequest);
            }            
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveRequest(int id, CancellationToken cancellationToken)
        {
            bool success = await _approvalRequestService.ApproveRequestAsync(id, cancellationToken);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }            
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectRequest(int id, [FromBody] string rejectionReason, CancellationToken cancellationToken)
        {
            bool success = await _approvalRequestService.RejectRequestAsync(id, rejectionReason, cancellationToken);

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

        [HttpPost("filter")]
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
