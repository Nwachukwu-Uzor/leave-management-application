using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<CreateLeaveTypeCommandHandler> _logger;
    public CreateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper, IAppLogger<CreateLeaveTypeCommandHandler> logger)
    {
        _leaveTypeRepository = leaveTypeRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        // validate incoming data 
        var validator = new CreateLeaveTypeCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.Errors.Any())
        {
            _logger.LogWarning(
               "Validation error for the Domain entity {0} - {1}",
               nameof(Domain.LeaveAllocation)
           );
            throw new BadRequestException("Invalid Leave type", validationResult);
        }
        var leaveEntity = _mapper.Map<Domain.LeaveType>(request);
        var data = await _leaveTypeRepository.CreateAsync(leaveEntity);
        return data.Id;
    }
}
