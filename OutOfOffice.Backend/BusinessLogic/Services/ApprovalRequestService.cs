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

                return approvalRequest.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an approval request");
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating approval request with Id: {id}");
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

                approvalRequest.Status = "Approved"; // Assuming "Approved" is a valid status
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while approving request with Id: {id}");
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

                approvalRequest.Status = "Rejected"; // Assuming "Rejected" is a valid status
                approvalRequest.Comment = rejectionReason;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while rejecting request with Id: {id}");
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting approval request with Id: {id}");
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
                _logger.LogError(ex, "Error occurred while searching for approval requests");
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
                _logger.LogError(ex, "Error occurred while filtering approval requests");
                return null;
            }
        }
    }
}
