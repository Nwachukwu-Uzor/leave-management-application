using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestsList;

public class LeaveRequestsListDto
{
    public int Id { get; set; }
    public LeaveTypeDto LeaveType { get; set; }
    public bool Approved { get; set; }
    public string RequestingEmployeeId { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime DateRequested { get; set; }
    public DateTime DateActioned { get; set; }
    public string? RequestComments { get; set; }
}
