using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;

namespace BusinessLogic.Services.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequestDTO>?> GetLeaveRequestsAsync(CancellationToken cancellationToken);
        Task<LeaveRequestDTO?> GetLeaveRequestByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateLeaveRequestAsync(CreateOrUpdateLeaveRequest request, CancellationToken cancellationToken);
        Task UpdateLeaveRequestAsync(int id, CreateOrUpdateLeaveRequest request, CancellationToken cancellationToken);
        Task SubmitOrCancelLeaveRequestAsync(int id, CancellationToken cancellationToken);
        Task DeleteLeaveRequestAsync(int id, CancellationToken cancellationToken);
        Task<List<LeaveRequestDTO>?> SearchLeaveRequestsAsync(string searchTerm, CancellationToken cancellationToken);
        Task<List<LeaveRequestDTO>?> FilterLeaveRequestsAsync(FilterOptions options, CancellationToken cancellationToken);
    }
}
