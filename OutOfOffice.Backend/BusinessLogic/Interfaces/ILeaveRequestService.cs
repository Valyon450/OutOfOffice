using BusinessLogic.Services;
using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequestDTO>?> GetLeaveRequestsAsync(CancellationToken cancellationToken);
        Task<LeaveRequestDTO?> GetLeaveRequestByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> AddOrUpdateLeaveRequestAsync(LeaveRequestDTO request, CancellationToken cancellationToken);
        Task<bool> CancelLeaveRequestAsync(int id, CancellationToken cancellationToken);
        Task<List<LeaveRequestDTO>?> SearchLeaveRequestsAsync(string searchTerm, CancellationToken cancellationToken);
        Task<List<LeaveRequestDTO>?> FilterLeaveRequestsAsync(FilterOptions options, CancellationToken cancellationToken);
    }
}
