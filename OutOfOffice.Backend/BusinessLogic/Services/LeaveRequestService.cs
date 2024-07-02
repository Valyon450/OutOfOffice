using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BusinessLogic.DTOs;
using DataAccess.Interfaces;

namespace BusinessLogic.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IMapper _mapper;

        public LeaveRequestService(OutOfOfficeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<LeaveRequestDTO>?> GetLeaveRequestsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var leaveRequests = await _context.LeaveRequests.ToListAsync(cancellationToken);
                return _mapper.Map<List<LeaveRequestDTO>>(leaveRequests);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task<LeaveRequestDTO?> GetLeaveRequestByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var leaveRequest = await _context.LeaveRequests.FindAsync(id, cancellationToken);
                return _mapper.Map<LeaveRequestDTO>(leaveRequest);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task AddOrUpdateLeaveRequestAsync(LeaveRequestDTO leaveRequestDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestDTO);

                if (leaveRequestDTO.Id == 0)
                {
                    _context.LeaveRequests.Add(leaveRequest);
                }
                else
                {
                    _context.LeaveRequests.Update(leaveRequest);
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
            }
        }

        public async Task CancelLeaveRequestAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var leaveRequest = await _context.LeaveRequests.FindAsync(id);
                if (leaveRequest != null)
                {
                    leaveRequest.Status = "Canceled"; // Assuming "Canceled" is a valid status
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
            }
        }

        public async Task<List<LeaveRequestDTO>?> SearchLeaveRequestsAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                var leaveRequests = await _context.LeaveRequests
                                        .Where(lr => lr.Id.ToString()
                                        .Contains(searchTerm))
                                        .ToListAsync(cancellationToken);

                return _mapper.Map<List<LeaveRequestDTO>>(leaveRequests);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task<List<LeaveRequestDTO>?> FilterLeaveRequestsAsync(FilterOptions options, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _context.LeaveRequests.AsQueryable();

                if (options.Status != null)
                    query = query.Where(lr => lr.Status == options.Status);

                // TODO: Add more filters based on options

                var leaveRequests = await query.ToListAsync(cancellationToken);

                return _mapper.Map<List<LeaveRequestDTO>>(leaveRequests);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
    }
}
