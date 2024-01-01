using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IAppLogger<UpdateLeaveAllocationCommand> _logger;
    private readonly IMapper _mapper;

    public UpdateLeaveAllocationCommandHandler(
         IMapper mapper,
         ILeaveAllocationRepository leaveAllocationRepository,
         ILeaveTypeRepository leaveTypeRepository,
         IAppLogger<UpdateLeaveAllocationCommand> logger)
    {
        _mapper = mapper;
        _leaveAllocationRepository = leaveAllocationRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _logger = logger;
    }
    public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateLeaveAllocationCommandValidator(_leaveTypeRepository, _leaveAllocationRepository);
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.Errors.Any())
        {
            _logger.LogWarning(
                "Validation error for the Domain entity {0} - {1}", 
                nameof(Domain.LeaveAllocation),
                request.Id
            );
            throw new BadRequestException("Invalid Leave Allocation", validationResult);
        }
        var existingLeaveAllocation = await _leaveAllocationRepository.GetByIdAsync(request.Id);
        if (existingLeaveAllocation == null)
        {
            _logger.LogWarning(
                "Request for domain entity {0} with key {1} was not found",
                nameof(Domain.LeaveAllocation), 
                request.Id
            );
            throw new NotFoundException("Leave Allocation with the key {0} was not found", request.Id);
        }
        var entity = _mapper.Map(request, existingLeaveAllocation);
        await _leaveAllocationRepository.UpdateAsync(entity);
        return Unit.Value;
    }
}
