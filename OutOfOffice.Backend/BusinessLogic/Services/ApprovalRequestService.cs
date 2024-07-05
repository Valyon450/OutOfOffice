using BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DataAccess.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;
using DataAccess.Entities;

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

        public async Task<List<ApprovalRequestDTO>?> GetApprovalRequestsAsync(CancellationToken cancellationToken)
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

        public async Task<ApprovalRequestDTO?> GetApprovalRequestByIdAsync(int id, CancellationToken cancellationToken)
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

        public async Task<int> CreateApprovalRequestAsync(CreateOrUpdateApprovalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: Validation

                var approvalRequest = _mapper.Map<ApprovalRequest>(request);

                _context.ApprovalRequests.Add(approvalRequest);

                await _context.SaveChangesAsync(cancellationToken);

                return approvalRequest.Id;
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                return 0;
            }
        }

        public async Task<bool> UpdateApprovalRequestAsync(int id, CreateOrUpdateApprovalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var existingApprovalRequest = await _context.ApprovalRequests.FindAsync(id);

                if (existingApprovalRequest == null)
                {
                    return false; // Approval request not found
                }

                // Map the updated properties from the request to the existing approval request
                _mapper.Map(request, existingApprovalRequest);

                // TODO: Validation

                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                return false;
            }
        }

        public async Task<bool> ApproveRequestAsync(int approvalRequestId, CancellationToken cancellationToken)
        {
            try
            {
                var approvalRequest = await _context.ApprovalRequests.FindAsync(approvalRequestId);

                if (approvalRequest == null)
                {
                    return false;
                }
                else
                {
                    approvalRequest.Status = "Approved"; // Assuming "Approved" is a valid status
                    await _context.SaveChangesAsync(cancellationToken);
                    return true;
                }
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                return false;
            }
        }

        public async Task<bool> RejectRequestAsync(int approvalRequestId, string rejectionReason, CancellationToken cancellationToken)
        {
            try
            {
                var approvalRequest = await _context.ApprovalRequests.FindAsync(approvalRequestId);
                if (approvalRequest == null)
                {
                    return false;
                }
                else
                {
                    approvalRequest.Status = "Rejected"; // Assuming "Rejected" is a valid status
                    approvalRequest.Comment = rejectionReason;
                    await _context.SaveChangesAsync(cancellationToken);
                    return true;
                }
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                return false;
            }            
        }

        public async Task<bool> DeleteApprovalRequestAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var approvalRequest = await _context.ApprovalRequests.FindAsync(id);

                if (approvalRequest == null)
                {
                    return false; // Approval request not found
                }

                _context.ApprovalRequests.Remove(approvalRequest);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                return false;
            }
        }

        public async Task<List<ApprovalRequestDTO>?> SearchApprovalRequestsAsync(string searchTerm, CancellationToken cancellationToken)
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

        public async Task<List<ApprovalRequestDTO>?> FilterApprovalRequestsAsync(FilterOptions options, CancellationToken cancellationToken)
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
