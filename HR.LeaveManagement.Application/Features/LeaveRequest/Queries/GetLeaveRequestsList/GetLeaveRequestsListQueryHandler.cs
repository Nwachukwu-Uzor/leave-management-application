using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestsList;

public class GetLeaveRequestsListQueryHandler : IRequestHandler<GetLeaveRequestsListQuery, List<LeaveRequestsListDto>>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IMapper _mapper;

    public GetLeaveRequestsListQueryHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _mapper = mapper;
    }
    public async Task<List<LeaveRequestsListDto>> Handle(GetLeaveRequestsListQuery request, CancellationToken cancellationToken)
    {
        // TODO: Check if employee is logged in
        var leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails();
        // TODO: Fill in the request with employee details
        var data = _mapper.Map<List<LeaveRequestsListDto>>(leaveRequests);
        return data;
    }
}
