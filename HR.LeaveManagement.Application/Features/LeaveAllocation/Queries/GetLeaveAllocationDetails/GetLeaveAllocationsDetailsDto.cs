using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;

public class GetLeaveAllocationsDetailsDto
{
    public int Id { get; set; }
    public int NumberOfDays { get; set; }
    public int LeaveTypeId { get; set; }
    public int Period { get; set; }
    public LeaveTypeDto? LeaveType { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
}
