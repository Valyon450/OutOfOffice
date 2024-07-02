using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class ApprovalRequestService : IApprovalRequestService
    {
        private readonly OutOfOfficeDbContext _context;

        public ApprovalRequestService(OutOfOfficeDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApprovalRequest>> GetApprovalRequestsAsync()
        {
            return await _context.ApprovalRequests.ToListAsync();
        }

        public async Task<ApprovalRequest> GetApprovalRequestByIdAsync(int id)
        {
            return await _context.ApprovalRequests.FindAsync(id);
        }

        public async Task ApproveRequestAsync(int approvalRequestId)
        {
            var request = await _context.ApprovalRequests.FindAsync(approvalRequestId);
            if (request != null)
            {
                request.Status = "Approved"; // Assuming "Approved" is a valid status
                await _context.SaveChangesAsync();
            }
        }

        public async Task RejectRequestAsync(int approvalRequestId, string rejectionReason)
        {
            var request = await _context.ApprovalRequests.FindAsync(approvalRequestId);
            if (request != null)
            {
                request.Status = "Rejected"; // Assuming "Rejected" is a valid status
                request.Comment = rejectionReason;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ApprovalRequest>> SearchApprovalRequestsAsync(string searchTerm)
        {
            var requests = await _context.ApprovalRequests
                .Where(ar => ar.ID.ToString().Contains(searchTerm))
                .ToListAsync();

            return requests;
        }

        public async Task<List<ApprovalRequest>> FilterApprovalRequestsAsync(FilterOptions options)
        {
            var query = _context.ApprovalRequests.AsQueryable();

            if (options.Status != null)
                query = query.Where(ar => ar.Status == options.Status);

            // Add more filters based on options as needed

            return await query.ToListAsync();
        }
    }
}
