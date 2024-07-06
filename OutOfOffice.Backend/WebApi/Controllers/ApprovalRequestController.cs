using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;
using BusinessLogic.Services.Interfaces;
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
