using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestsList;

public record GetLeaveRequestsListQuery : IRequest<List<LeaveRequestsListDto>>;
