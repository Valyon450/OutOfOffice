using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BusinessLogic.DTOs;
using DataAccess.Interfaces;
using BusinessLogic.Options;
using BusinessLogic.Requests;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.ValidationServices.Interfaces;

namespace BusinessLogic.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILeaveRequestValidationService _leaveRequestValidationService;
        private readonly ILogger<LeaveRequestService> _logger;

        public LeaveRequestService(IOutOfOfficeDbContext context, IMapper mapper, ILeaveRequestValidationService leaveRequestValidationService, ILogger<LeaveRequestService> logger)
        {
            _context = context;
            _mapper = mapper;
            _leaveRequestValidationService = leaveRequestValidationService;
            _logger = logger;
        }

        public async Task<List<LeaveRequestDTO>?> GetLeaveRequestsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var leaveRequests = await _context.LeaveRequest.ToListAsync(cancellationToken);
                return _mapper.Map<List<LeaveRequestDTO>>(leaveRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching leave requests");
                return null;
            }
        }

        public async Task<LeaveRequestDTO?> GetLeaveRequestByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var leaveRequest = await _context.LeaveRequest.FindAsync(new object[] { id }, cancellationToken);

                if (leaveRequest == null)
                {
                    throw new Exception($"Leave Request with Id: {id} not found.");
                }

                return _mapper.Map<LeaveRequestDTO>(leaveRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching leave request with Id: {id}");
                throw;
            }
        }

        public async Task<int> CreateLeaveRequestAsync(CreateOrUpdateLeaveRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _leaveRequestValidationService.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var leaveRequest = _mapper.Map<LeaveRequest>(request);

                _context.LeaveRequest.Add(leaveRequest);

                await _context.SaveChangesAsync(cancellationToken);

                return leaveRequest.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a leave request");
                throw;
            }
        }

        public async Task UpdateLeaveRequestAsync(int id, CreateOrUpdateLeaveRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var existingLeaveRequest = await _context.LeaveRequest.FindAsync(new object[] { id }, cancellationToken);

                if (existingLeaveRequest == null)
                {
                    throw new Exception($"Leave Request with Id: {id} not found.");
                }

                var validationResult = await _leaveRequestValidationService.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                // Map the updated properties from the request to the existing leave request
                _mapper.Map(request, existingLeaveRequest);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating leave request with Id: {id}");
                throw;
            }
        }

        public async Task CancelLeaveRequestAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var leaveRequest = await _context.LeaveRequest.FindAsync(new object[] { id }, cancellationToken);

                if (leaveRequest == null)
                {
                    throw new Exception($"Leave Request with Id: {id} not found.");
                }

                leaveRequest.Status = "Canceled"; // Assuming "Canceled" is a valid status
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while canceling leave request with Id: {id}");
                throw;
            }
        }

        public async Task DeleteLeaveRequestAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var leaveRequest = await _context.LeaveRequest.FindAsync(new object[] { id }, cancellationToken);

                if (leaveRequest == null)
                {
                    throw new Exception($"Leave Request with Id: {id} not found.");
                }

                _context.LeaveRequest.Remove(leaveRequest);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting leave request with Id: {id}");
                throw;
            }
        }

        public async Task<List<LeaveRequestDTO>?> SearchLeaveRequestsAsync(string searchTerm, CancellationToken cancellationToken)
        {
            try
            {
                var leaveRequests = await _context.LeaveRequest
                    .Where(lr => lr.AbsenceReason.Contains(searchTerm))
                    .ToListAsync(cancellationToken);

                return _mapper.Map<List<LeaveRequestDTO>>(leaveRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for leave requests");
                return null;
            }
        }

        public async Task<List<LeaveRequestDTO>?> FilterLeaveRequestsAsync(FilterOptions options, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.LeaveRequest.AsQueryable();

                if (options.Status != null)
                    query = query.Where(lr => lr.Status == options.Status);

                //TODO Add more filters based on options

                var leaveRequests = await query.ToListAsync(cancellationToken);

                return _mapper.Map<List<LeaveRequestDTO>>(leaveRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while filtering leave requests");
                return null;
            }
        }    
    }
}
