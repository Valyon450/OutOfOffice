using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Options;
using BusinessLogic.Requests;
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

        [HttpPost]
        public async Task<IActionResult> CreateApprovalRequest([FromBody] CreateOrUpdateApprovalRequest request, CancellationToken cancellationToken)
        {
            int id = await _approvalRequestService.CreateApprovalRequestAsync(request, cancellationToken);

            if (id > 0)
            {
                return CreatedAtAction(nameof(GetApprovalRequestById), new { id }, request);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApprovalRequest(int id, [FromBody] CreateOrUpdateApprovalRequest request, CancellationToken cancellationToken)
        {
            bool success = await _approvalRequestService.UpdateApprovalRequestAsync(id, request, cancellationToken);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch("{id}/approve")]
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

        [HttpPatch("{id}/reject")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApprovalRequest(int id, CancellationToken cancellationToken)
        {
            bool success = await _approvalRequestService.DeleteApprovalRequestAsync(id, cancellationToken);

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

        [HttpGet("filter")]
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
