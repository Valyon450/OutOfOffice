using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly OutOfOfficeDbContext _context;

        public LeaveRequestService(OutOfOfficeDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsAsync()
        {
            return await _context.LeaveRequests.ToListAsync();
        }

        public async Task<LeaveRequest> GetLeaveRequestByIdAsync(int id)
        {
            return await _context.LeaveRequests.FindAsync(id);
        }

        public async Task AddOrUpdateLeaveRequestAsync(LeaveRequest request)
        {
            if (request.ID == 0)
                _context.LeaveRequests.Add(request);
            else
                _context.LeaveRequests.Update(request);

            await _context.SaveChangesAsync();
        }

        public async Task CancelLeaveRequestAsync(int id)
        {
            var request = await _context.LeaveRequests.FindAsync(id);
            if (request != null)
            {
                request.Status = "Canceled"; // Assuming "Canceled" is a valid status
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<LeaveRequest>> SearchLeaveRequestsAsync(string searchTerm)
        {
            var requests = await _context.LeaveRequests
                .Where(lr => lr.ID.ToString().Contains(searchTerm))
                .ToListAsync();

            return requests;
        }

        public async Task<List<LeaveRequest>> FilterLeaveRequestsAsync(FilterOptions options)
        {
            var query = _context.LeaveRequests.AsQueryable();

            if (options.Status != null)
                query = query.Where(lr => lr.Status == options.Status);

            // Add more filters based on options as needed

            return await query.ToListAsync();
        }
    }
}
