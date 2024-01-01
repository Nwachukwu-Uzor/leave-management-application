using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;

public class CreateLeaveAllocationCommand : IRequest<int>
{
    public int LeaveTypeId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
}
