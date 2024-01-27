using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, int>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;
    private readonly IAppLogger<CreateLeaveRequestCommandHandler> _logger;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    public CreateLeaveRequestCommandHandler(
        ILeaveRequestRepository leaveRequestRepository,
        IMapper mapper,
        ILeaveTypeRepository leaveTypeRepository,
        IEmailSender emailSender,
        IAppLogger<CreateLeaveRequestCommandHandler> logger,
        IUserService userService,
        IHttpContextAccessor httpContextAccessor,
        ILeaveAllocationRepository leaveAllocationRepository)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _mapper = mapper;
        _leaveTypeRepository = leaveTypeRepository;
        _emailSender = emailSender;
        _logger = logger;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _leaveAllocationRepository = leaveAllocationRepository;
    }
    public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.Errors.Any())
        {
            throw new BadRequestException("Validation failed for creating leave request", validationResult);
        }
       
        // Get User for the leave request
        var employeeId = _httpContextAccessor.HttpContext?.User.FindFirst("uuid")?.ToString();
        if (employeeId == null)
        {
            throw new BadRequestException("Unauthorized");
        }
        var user = await _userService.GetEmployee(employeeId);
        if (user == null)
        {
            throw new BadRequestException("Unauthorized");
        }

        // Check employees allocation
        var employeesLeaveAllocation = await _leaveAllocationRepository.GetUsersLeaveAllocations(employeeId, request.LeaveTypeId);

        if (!employeesLeaveAllocation.Any() || employeesLeaveAllocation is null)
        {
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
                nameof(request.LeaveTypeId),
                "You do not  have any allocations for this this"
            ));
            throw new BadRequestException("Invalid leave request", validationResult);
        }

        var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
        leaveRequest.RequestingEmployeeId = employeeId;
        var response = await _leaveRequestRepository.CreateAsync(leaveRequest);
        // Send confirmation message
        try
        {
            var confirmationEmail = new EmailMessage
            {
                To = string.Empty,
                Subject = "Leave Request Update Successfully",
                Body = $"You leave request from {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} " +
                  $"was updated successfully"
            };
            await _emailSender.SendEmail(confirmationEmail);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
        }

        return response.Id;
    }
}
