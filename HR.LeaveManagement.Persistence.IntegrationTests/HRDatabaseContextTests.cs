using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DataBaseContexts;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace HR.LeaveManagement.Persistence.IntegrationTests
{
    public class HRDatabaseContextTests
    {
        private readonly HRDatabaseContext _hrDatabaseContext;
        public HRDatabaseContextTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<HRDatabaseContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _hrDatabaseContext = new HRDatabaseContext(dbContextOptions);
        }

        [Fact]
        public async Task Save_SetsDateCreatedValue()
        {
            var leaveType = new LeaveType
            {
                Name = "Test",
                DefaultDays = 5,
                Id = 1
            };

            await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();

            leaveType.DateCreated.ShouldNotBe(null);
        }


        [Fact]
        public async Task Save_SetsDateDateModifiedValue()
        {
            var leaveType = new LeaveType
            {
                Name = "Test",
                DefaultDays = 5,
                Id = 1
            };

            await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();

            leaveType.DateModified.ShouldNotBe(null);
        }
    }
}