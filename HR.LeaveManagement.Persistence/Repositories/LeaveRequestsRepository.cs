using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DataBaseContexts;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveRequestsRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
{
    public LeaveRequestsRepository(HRDatabaseContext context) : base(context)
    {
    }
}
