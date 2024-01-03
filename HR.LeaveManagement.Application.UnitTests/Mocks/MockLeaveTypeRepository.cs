using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Moq;

namespace HR.LeaveManagement.Application.UnitTests.Mocks;

public class MockLeaveTypeRepository
{
    public static Mock<ILeaveTypeRepository> GetMockLeaveTypes()
    {
        var leaveTypes = new List<LeaveType>
        {
            new LeaveType
            {
                Id = 1,
                Name = "Test Vacation"
            },
            new LeaveType
            {
                Id = 2,
                Name = "Test Sick"
            },
            new LeaveType
            {
                Id = 3,
                Name = "Test Maternity"
            }
        };
        var mockLeaveTypeRepo = new Mock<ILeaveTypeRepository>();
        mockLeaveTypeRepo.Setup(r => r.GetAllAsync())
                         .ReturnsAsync(leaveTypes);
        mockLeaveTypeRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveType>()))
            .Returns((LeaveType leaveType) =>
            {
                leaveType.Id = 4;
                leaveTypes.Add(leaveType);
                return Task.FromResult(leaveType);
            });
        return mockLeaveTypeRepo;
    }
}
