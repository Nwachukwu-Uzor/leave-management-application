using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest;

public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand, Unit>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IAppLogger<DeleteLeaveRequestCommandHandler> _logger;

    public DeleteLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, IAppLogger<DeleteLeaveRequestCommandHandler> logger)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequestToDelete = await _leaveRequestRepository.GetByIdAsync(request.Id);
        if (leaveRequestToDelete == null)
        {
            _logger.LogWarning(
                "The domain entity {0} with key {1} was not found", 
                nameof(Domain.LeaveRequest), 
                request.Id
            );
            throw new NotFoundException(nameof(Domain.LeaveRequest), request.Id);
        }
        await _leaveRequestRepository.DeleteAsync(leaveRequestToDelete);
        return Unit.Value;
    }
}
