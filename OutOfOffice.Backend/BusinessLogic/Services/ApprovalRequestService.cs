using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DataAccess.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;
using DataAccess.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.ValidationServices.Interfaces;

namespace BusinessLogic.Services
{
    public class ApprovalRequestService : IApprovalRequestService
    {
        private readonly IOutOfOfficeDbContext _context;
        private readonly IMapper _mapper;
        private readonly IApprovalRequestValidationService _approvalRequestValidationService;
        private readonly ILogger<ApprovalRequestService> _logger;

        public ApprovalRequestService(IOutOfOfficeDbContext context, IMapper mapper, IApprovalRequestValidationService approvalRequestValidationService, ILogger<ApprovalRequestService> logger)
        {
            _context = context;
            _mapper = mapper;
            _approvalRequestValidationService = approvalRequestValidationService;
            _logger = logger;
        }

        public async Task<List<ApprovalRequestDTO>?> GetApprovalRequestsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var approvalRequests = await _context.ApprovalRequest.ToListAsync(cancellationToken);
                return _mapper.Map<List<ApprovalRequestDTO>>(approvalRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching approval requests");
                return null;
            }
        }

        public async Task<ApprovalRequestDTO?> GetApprovalRequestByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var approvalRequest = await _context.ApprovalRequest.FindAsync(new object[] { id }, cancellationToken);

                if (approvalRequest == null)
                {
                    throw new Exception($"Approval request with Id: {id} not found.");
                }

                return _mapper.Map<ApprovalRequestDTO>(approvalRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching approval request with Id: {id}");
                throw;
            }
        }

        public async Task<int> CreateApprovalRequestAsync(CreateOrUpdateApprovalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _approvalRequestValidationService.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var approvalRequest = _mapper.Map<ApprovalRequest>(request);

                _context.ApprovalRequest.Add(approvalRequest);

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Approval request with Id: {approvalRequest.Id} has been created successfully.");

                return approvalRequest.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an approval request.");
                throw;
            }
        }

        public async Task UpdateApprovalRequestAsync(int id, CreateOrUpdateApprovalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var existingApprovalRequest = await _context.ApprovalRequest.FindAsync(new object[] { id }, cancellationToken);

                if (existingApprovalRequest == null)
                {
                    throw new Exception($"Approval request with Id: {id} not found.");
                }

                var validationResult = await _approvalRequestValidationService.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                // Map the updated properties from the request to the existing approval request
                _mapper.Map(request, existingApprovalRequest);

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Approval request with Id: {id} has been updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating approval request with Id: {id}.");
                throw;
            }
        }

        public async Task ApproveRequestAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var approvalRequest = await _context.ApprovalRequest.FindAsync(new object[] { id }, cancellationToken);

                if (approvalRequest == null)
                {
                    throw new Exception($"Approval request with Id: {id} not found.");
                }

                var leaveRequest = await _context.LeaveRequest.FindAsync(new object[] { approvalRequest.LeaveRequestId }, cancellationToken);
                if (leaveRequest == null)
                {
                    throw new Exception($"Related leave request with Id: {approvalRequest.LeaveRequestId} not found.");
                }

                approvalRequest.Status = "Approved";
                leaveRequest.Status = "Approved";

                // Перерахунок балансу відсутності
                var employee = await _context.Employee.FindAsync(new object[] { leaveRequest.EmployeeId }, cancellationToken);
                if (employee == null)
                {
                    throw new Exception($"Related employee with Id: {leaveRequest.EmployeeId} not found.");
                }

                int absenceDays = (int) (leaveRequest.EndDate - leaveRequest.StartDate).TotalDays + 1; // Add 1 to include the end day

                // Оновлення балансу відсутності
                employee.OutOfOfficeBalance += absenceDays;

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Leave request with Id: {approvalRequest.LeaveRequestId} has been approved successfully. Approval request Id: {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while approving request with Id: {id}.");
                throw;
            }
        }

        public async Task RejectRequestAsync(int id, string rejectionReason, CancellationToken cancellationToken)
        {
            try
            {
                var approvalRequest = await _context.ApprovalRequest.FindAsync(new object[] { id }, cancellationToken);

                if (approvalRequest == null)
                {
                    throw new Exception($"Approval request with Id: {id} not found.");
                }

                var leaveRequest = await _context.LeaveRequest.FindAsync(new object[] { approvalRequest.LeaveRequestId }, cancellationToken);
                if (leaveRequest == null)
                {
                    throw new Exception($"Related leave request with Id: {approvalRequest.LeaveRequestId} not found.");
                }

                approvalRequest.Status = "Rejected";
                approvalRequest.Comment = rejectionReason;
                leaveRequest.Status = "Rejected";

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Leave request with Id: {approvalRequest.LeaveRequestId} has been rejected successfully. Approval request Id: {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while rejecting request with Id: {id}.");
                throw;
            }
        }

        public async Task DeleteApprovalRequestAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var approvalRequest = await _context.ApprovalRequest.FindAsync(new object[] { id }, cancellationToken);

                if (approvalRequest == null)
                {
                    throw new Exception($"Approval request with Id: {id} not found.");
                }

                _context.ApprovalRequest.Remove(approvalRequest);

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Approval request with Id: {id} has been deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting approval request with Id: {id}.");
                throw;
            }
        }

        public async Task<List<ApprovalRequestDTO>?> SearchApprovalRequestsAsync(string searchTerm, CancellationToken cancellationToken)
        {
            try
            {
                var approvalRequests = await _context.ApprovalRequest
                    .Where(ar => ar.Id.ToString().Contains(searchTerm))
                    .ToListAsync(cancellationToken);

                return _mapper.Map<List<ApprovalRequestDTO>>(approvalRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for approval requests.");
                return null;
            }
        }

        public async Task<List<ApprovalRequestDTO>?> FilterApprovalRequestsAsync(FilterOptions options, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.ApprovalRequest.AsQueryable();

                if (options.Status != null)
                    query = query.Where(ar => ar.Status == options.Status);

                //TODO Add more filters based on options

                var approvalRequests = await query.ToListAsync(cancellationToken);

                return _mapper.Map<List<ApprovalRequestDTO>>(approvalRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while filtering approval requests.");
                return null;
            }
        }
    }
}
