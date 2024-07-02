using BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DataAccess.Interfaces;
using BusinessLogic.DTOs;

namespace BusinessLogic.Services
{
    public class ApprovalRequestService : IApprovalRequestService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IMapper _mapper;

        public ApprovalRequestService(IOutOfOfficeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ApprovalRequestDTO>?> GetApprovalRequestsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var approvalRequests = await _context.ApprovalRequests.ToListAsync(cancellationToken);
                return _mapper.Map<List<ApprovalRequestDTO>>(approvalRequests);
            }
            catch (OperationCanceledException)
            {
                return null;
            }            
        }

        public async Task<ApprovalRequestDTO?> GetApprovalRequestByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var approvalRequest = await _context.ApprovalRequests.FindAsync(id, cancellationToken);
                return _mapper.Map<ApprovalRequestDTO>(approvalRequest);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task ApproveRequestAsync(int approvalRequestId, CancellationToken cancellationToken = default)
        {
            try
            {
                var approvalRequest = await _context.ApprovalRequests.FindAsync(approvalRequestId);
                if (approvalRequest != null)
                {
                    approvalRequest.Status = "Approved"; // Assuming "Approved" is a valid status
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
            }
        }

        public async Task RejectRequestAsync(int approvalRequestId, string rejectionReason, CancellationToken cancellationToken = default)
        {
            try
            {
                var approvalRequest = await _context.ApprovalRequests.FindAsync(approvalRequestId);
                if (approvalRequest != null)
                {
                    approvalRequest.Status = "Rejected"; // Assuming "Rejected" is a valid status
                    approvalRequest.Comment = rejectionReason;
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
            }            
        }

        public async Task<List<ApprovalRequestDTO>?> SearchApprovalRequestsAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                var approvalRequests = await _context.ApprovalRequests
                .Where(ar => ar.Id.ToString().Contains(searchTerm))
                .ToListAsync(cancellationToken);

                return _mapper.Map<List<ApprovalRequestDTO>>(approvalRequests);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task<List<ApprovalRequestDTO>?> FilterApprovalRequestsAsync(FilterOptions options, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _context.ApprovalRequests.AsQueryable();

                if (options.Status != null)
                    query = query.Where(ar => ar.Status == options.Status);

                //TODO Add more filters based on options

                var approvalRequests = await query.ToListAsync(cancellationToken);

                return _mapper.Map<List<ApprovalRequestDTO>>(query);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
    }
}
