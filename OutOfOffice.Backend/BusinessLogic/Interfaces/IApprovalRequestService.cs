using BusinessLogic.Services;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IApprovalRequestService
    {
        Task<List<ApprovalRequest>> GetApprovalRequestsAsync();
        Task<ApprovalRequest> GetApprovalRequestByIdAsync(int id);
        Task ApproveRequestAsync(int approvalRequestId);
        Task RejectRequestAsync(int approvalRequestId, string rejectionReason);
        Task<List<ApprovalRequest>> SearchApprovalRequestsAsync(string searchTerm);
        Task<List<ApprovalRequest>> FilterApprovalRequestsAsync(FilterOptions options);
    }
}
