using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;

namespace BusinessLogic.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequestDTO>?> GetLeaveRequestsAsync(CancellationToken cancellationToken);
        Task<LeaveRequestDTO?> GetLeaveRequestByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateLeaveRequestAsync(CreateOrUpdateLeaveRequest request, CancellationToken cancellationToken);
        Task<bool> UpdateLeaveRequestAsync(int id, CreateOrUpdateLeaveRequest request, CancellationToken cancellationToken);
        Task<bool> CancelLeaveRequestAsync(int id, CancellationToken cancellationToken);
        Task<bool> DeleteLeaveRequestAsync(int id, CancellationToken cancellationToken);
        Task<List<LeaveRequestDTO>?> SearchLeaveRequestsAsync(string searchTerm, CancellationToken cancellationToken);
        Task<List<LeaveRequestDTO>?> FilterLeaveRequestsAsync(FilterOptions options, CancellationToken cancellationToken);
    }
}
