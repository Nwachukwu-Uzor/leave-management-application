using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Persistence
{
    public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
    {
        Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id);
        Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails();
        Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId);
        Task<bool> LeaveAllocationExists(string userId, int leaveTypeId, int period);
        Task AddLeaveAllocations(List<LeaveAllocation> leaveAllocations);
        Task<List<LeaveAllocation>> GetUsersLeaveAllocations(string userId, int leaveTypeId);
    }
}