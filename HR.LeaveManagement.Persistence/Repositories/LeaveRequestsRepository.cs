using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DataBaseContexts;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveRequestsRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
{
    public LeaveRequestsRepository(HRDatabaseContext context) : base(context)
    {
    }

    public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails()
    {
        var leaveRequests = await _context.LeaveRequests
            .Include(d => d.LeaveType)
            .AsNoTracking().ToListAsync();
        return leaveRequests;
    }

    public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails(string userId)
    {
        var leaveRequests = await _context.LeaveRequests
            .Where(leaveRequest => leaveRequest.RequestingEmployeeId == userId)
            .Include(d => d.LeaveType)
            .AsNoTracking().ToListAsync();
        return leaveRequests;
    }

    public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
    {
        var leaveRequest = await _context.LeaveRequests
            .Include(d => d.LeaveType).AsNoTracking()
            .FirstOrDefaultAsync(request => request.Id == id);
        return leaveRequest;
    }
}
