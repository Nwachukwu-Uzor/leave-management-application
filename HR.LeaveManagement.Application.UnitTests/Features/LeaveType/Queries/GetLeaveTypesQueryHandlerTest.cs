using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveType.Queries;

public class GetLeaveTypesQueryHandlerTest
{
    private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepository;
    private readonly IMapper _mapper;
    private readonly Mock<IAppLogger<GetLeaveTypesQueryHandler>> _mockAppLogger;

    public GetLeaveTypesQueryHandlerTest()
    {
        _mockLeaveTypeRepository = MockLeaveTypeRepository.GetMockLeaveTypes();
        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<LeaveTypeProfile>();
        });
        _mapper = mapperConfig.CreateMapper();
        _mockAppLogger = new Mock<IAppLogger<GetLeaveTypesQueryHandler>>();
    }

    [Fact]
    public async Task GetLeaveTypesTest()
    {
        var handler = new GetLeaveTypesQueryHandler(_mockLeaveTypeRepository.Object, _mapper, _mockAppLogger.Object);
        var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);
        result.ShouldBeOfType<List<LeaveTypeDto>>();
        result.Count.ShouldBe(3);
    }
}
