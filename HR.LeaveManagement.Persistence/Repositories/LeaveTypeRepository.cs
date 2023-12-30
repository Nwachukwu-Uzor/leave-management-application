using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DataBaseContexts;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
{
    public LeaveTypeRepository(HRDatabaseContext context) : base(context)
    {
    }

    public async Task<bool> IsLeaveTypeNameUnique(string name)
    {
        var leaveTypeExist = await _context.LeaveTypes.AnyAsync(leaveType => leaveType.Name == name);
        return !leaveTypeExist;
    }
}
