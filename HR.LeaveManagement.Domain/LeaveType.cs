using HR.LeaveManagement.Domain.Common;

namespace HR.LeaveManagement.Domain;

public class LeaveType : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public int DefaultDays { get; set; }
}