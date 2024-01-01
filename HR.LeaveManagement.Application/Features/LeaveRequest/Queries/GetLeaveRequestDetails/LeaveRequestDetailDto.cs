using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails;

public class LeaveRequestDetailDto
{
    public int Id { get; set; }
    public LeaveTypeDto LeaveType { get; set; }
    public int LeaveTypeId { get; set; }
    public bool Approved { get; set; }
    public bool Cancelled { get; set; }
    public string RequestingEmployeeId { get; set; } = string.Empty;
    public DateTime DateRequested { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
