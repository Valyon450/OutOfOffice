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
        private readonly IApprovalRequestService _approvalRequestService;
        private readonly ILogger<LeaveRequestService> _logger;

        public LeaveRequestService(IOutOfOfficeDbContext context, IMapper mapper, ILeaveRequestValidationService leaveRequestValidationService, IApprovalRequestService approvalRequestService, ILogger<LeaveRequestService> logger)
        {
            _context = context;
            _mapper = mapper;
            _leaveRequestValidationService = leaveRequestValidationService;
            _approvalRequestService = approvalRequestService;
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
                _logger.LogError(ex, "Error occurred while fetching leave requests.");
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
                _logger.LogError(ex, $"Error occurred while fetching leave request with Id: {id}.");
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

                _logger.LogInformation($"Leave request with Id: {leaveRequest.Id} has been created successfully.");

                return leaveRequest.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a leave request.");
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

                _logger.LogInformation($"Leave request with Id: {id} has been updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating leave request with Id: {id}");
                throw;
            }
        }

        public async Task SubmitOrCancelLeaveRequestAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var leaveRequest = await _context.LeaveRequest.FindAsync(new object[] { id }, cancellationToken);

                if (leaveRequest == null)
                {
                    throw new Exception($"Leave Request with Id: {id} not found.");
                }

                if (leaveRequest.Status == "New" || leaveRequest.Status == "Canceled")
                {
                    leaveRequest.Status = "Submitted";

                    var employee = await _context.Employee.FindAsync(new object[] { leaveRequest.EmployeeId }, cancellationToken);

                    if (employee == null)
                    {
                        throw new Exception($"Related employee with Id: {leaveRequest.EmployeeId} not found.");
                    }

                    if (employee.PeoplePartnerId == null)
                    {
                        throw new Exception($"HR Manager not assigned for employee with Id: {leaveRequest.EmployeeId}.");
                    }

                    int HRManagerId = (int)employee.PeoplePartnerId;

                    var approvalRequest = new CreateOrUpdateApprovalRequest
                    {
                        ApproverId = HRManagerId,
                        LeaveRequestId = leaveRequest.Id,
                        Status = "New"
                    };

                    await _approvalRequestService.CreateApprovalRequestAsync(approvalRequest, cancellationToken);
                }
                else if (leaveRequest.Status == "Submitted")
                {
                    leaveRequest.Status = "Canceled";

                    var approvalRequests = await _context.ApprovalRequest
                        .Where(ar => ar.LeaveRequestId == leaveRequest.Id)
                        .ToListAsync(cancellationToken);

                    _context.ApprovalRequest.RemoveRange(approvalRequests);
                }                

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Leave request with Id: {id} get status change successfully. Status: {leaveRequest.Status}");
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

                _logger.LogInformation($"Leave request with Id: {id} has been deleted successfully.");
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
                _logger.LogError(ex, "Error occurred while searching for leave requests.");
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
                _logger.LogError(ex, "Error occurred while filtering leave requests.");
                return null;
            }
        }    
    }
}
