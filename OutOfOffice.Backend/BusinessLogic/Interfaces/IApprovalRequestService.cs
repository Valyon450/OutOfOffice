using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;

namespace BusinessLogic.Interfaces
{
    public interface IApprovalRequestService
    {
        Task<List<ApprovalRequestDTO>?> GetApprovalRequestsAsync(CancellationToken cancellationToken);
        Task<ApprovalRequestDTO?> GetApprovalRequestByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateApprovalRequestAsync(CreateOrUpdateApprovalRequest request, CancellationToken cancellationToken);
        Task<bool> UpdateApprovalRequestAsync(int id, CreateOrUpdateApprovalRequest request, CancellationToken cancellationToken);
        Task<bool> ApproveRequestAsync(int id, CancellationToken cancellationToken);
        Task<bool> RejectRequestAsync(int id, string rejectionReason, CancellationToken cancellationToken);
        Task<bool> DeleteApprovalRequestAsync(int id, CancellationToken cancellationToken);
        Task<List<ApprovalRequestDTO>?> SearchApprovalRequestsAsync(string searchTerm, CancellationToken cancellationToken);
        Task<List<ApprovalRequestDTO>?> FilterApprovalRequestsAsync(FilterOptions options, CancellationToken cancellationToken);
    }
}
