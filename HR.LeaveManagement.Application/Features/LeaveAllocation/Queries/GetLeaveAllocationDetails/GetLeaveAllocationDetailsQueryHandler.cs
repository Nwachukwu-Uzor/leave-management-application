using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;

public class GetLeaveAllocationDetailsQueryHandler : IRequestHandler<GetLeaveAllocationDetailsQuery, GetLeaveAllocationsDetailsDto>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<GetLeaveAllocationDetailsQueryHandler> _logger;
    public GetLeaveAllocationDetailsQueryHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper, IAppLogger<GetLeaveAllocationDetailsQueryHandler> logger)
    {
        _leaveAllocationRepository = leaveAllocationRepository;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task<GetLeaveAllocationsDetailsDto> Handle(GetLeaveAllocationDetailsQuery request, CancellationToken cancellationToken)
    {
        var leaveAllocation = await _leaveAllocationRepository.GetLeaveAllocationWithDetails(request.Id);
        if (leaveAllocation == null)
        {
            _logger.LogWarning("Domain entity {0} with the key {1} was not found", nameof(LeaveAllocation), request.Id);
            throw new NotFoundException(nameof(Domain.LeaveAllocation), request.Id);
        }
        var data = _mapper.Map<GetLeaveAllocationsDetailsDto>(leaveAllocation);
        return data;
    }
}
