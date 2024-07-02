using BusinessLogic.Services;
using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IApprovalRequestService
    {
        Task<List<ApprovalRequestDTO>?> GetApprovalRequestsAsync(CancellationToken cancellationToken);
        Task<ApprovalRequestDTO?> GetApprovalRequestByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> ApproveRequestAsync(int approvalRequestId, CancellationToken cancellationToken);
        Task<bool> RejectRequestAsync(int approvalRequestId, string rejectionReason, CancellationToken cancellationToken);
        Task<List<ApprovalRequestDTO>?> SearchApprovalRequestsAsync(string searchTerm, CancellationToken cancellationToken);
        Task<List<ApprovalRequestDTO>?> FilterApprovalRequestsAsync(FilterOptions options, CancellationToken cancellationToken);
    }
}
