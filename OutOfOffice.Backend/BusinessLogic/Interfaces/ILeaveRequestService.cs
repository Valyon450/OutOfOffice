using BusinessLogic.Services;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequest>> GetLeaveRequestsAsync();
        Task<LeaveRequest> GetLeaveRequestByIdAsync(int id);
        Task AddOrUpdateLeaveRequestAsync(LeaveRequest request);
        Task CancelLeaveRequestAsync(int id);
        Task<List<LeaveRequest>> SearchLeaveRequestsAsync(string searchTerm);
        Task<List<LeaveRequest>> FilterLeaveRequestsAsync(FilterOptions options);
    }
}
