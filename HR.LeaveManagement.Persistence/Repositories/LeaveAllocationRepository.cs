using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DataBaseContexts;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
{
    public LeaveAllocationRepository(HRDatabaseContext context) : base(context)
    {
    }

    public async Task AddLeaveAllocations(List<LeaveAllocation> leaveAllocations)
    {
        await _context.LeaveAllocations.AddRangeAsync(leaveAllocations);
        await _context.SaveChangesAsync();
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
    {
        var allocations = await _context.LeaveAllocations
            .Include(allocation => allocation.LeaveType)
            .AsNoTracking().ToListAsync();
        return allocations;
    }

    public async  Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId)
    {
        var allocations = await _context.LeaveAllocations
            .Where(allocation => allocation.EmployeeId == userId)
           .Include(allocation => allocation.LeaveType)
           .AsNoTracking().ToListAsync();
        return allocations;
    }

    public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
    {
        var allocation = await _context.LeaveAllocations
            .Include(allocation => allocation.LeaveType)
            .AsNoTracking().FirstOrDefaultAsync(allocation => allocation.Id == id);
        return allocation;
    }

    public async Task<List<LeaveAllocation>> GetUsersLeaveAllocations(string userId, int leaveTypeId)
    {
        var allocations = await _context.LeaveAllocations
            .Where(allocation => 
                allocation.EmployeeId == userId
                &&
                allocation.LeaveTypeId == leaveTypeId
            )
           .Include(allocation => allocation.LeaveType)
           .AsNoTracking().ToListAsync();
        return allocations;
    }

    public async Task<bool> LeaveAllocationExists(string userId, int leaveTypeId, int period)
    {
        var allocationExists = await _context.LeaveAllocations
           .AnyAsync(allocation => 
                allocation.EmployeeId == userId 
                && 
                allocation.LeaveTypeId == leaveTypeId
                &&
                allocation.Period == period
           );
        return allocationExists;
    }
}
