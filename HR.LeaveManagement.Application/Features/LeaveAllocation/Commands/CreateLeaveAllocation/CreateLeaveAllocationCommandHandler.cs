using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;

public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Unit>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;
    private IUserService _userService;

    public CreateLeaveAllocationCommandHandler(
        IMapper mapper,
        ILeaveAllocationRepository leaveAllocationRepository,
        ILeaveTypeRepository leaveTypeRepository
,
        IUserService userService)
    {
        _mapper = mapper;
        _leaveAllocationRepository = leaveAllocationRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _userService = userService;
    }

    public async Task<Unit> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.Errors.Any())
        {
            throw new BadRequestException("Invalid Leave Allocation", validationResult);
        }
        // Get Leave Type for allocation
        var leaveTypeToAllocate = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId);
        //      1. Find Employees
        var employees = await _userService.GetEmployees();
        //      2. Add leave period
        var period = DateTime.Now.Year;
        // Assign allocation if allocation does not already exist for the leave type and the period
        var allocations = new List<Domain.LeaveAllocation>();
        foreach(var employee in employees )
        {
            var allocationExists = await _leaveAllocationRepository.LeaveAllocationExists(employee.Id, request.LeaveTypeId, period);
            if (!allocationExists)
            {
                var newAllocation = new Domain.LeaveAllocation()
                {
                    LeaveTypeId = request.LeaveTypeId,
                    EmployeeId = employee.Id,
                    Period = period,
                    NumberOfDays = leaveTypeToAllocate.DefaultDays
                };
                allocations.Add(newAllocation);
            }
        }
        if (allocations.Any())
        {
            await _leaveAllocationRepository.AddLeaveAllocations(allocations);
        }
        
        return Unit.Value;
    }
}
