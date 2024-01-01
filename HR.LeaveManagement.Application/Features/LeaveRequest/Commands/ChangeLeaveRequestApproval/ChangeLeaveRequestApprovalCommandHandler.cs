using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;

public class ChangeLeaveRequestApprovalCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Unit>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IEmailSender _emailSender;
    private readonly IAppLogger<CancelLeaveRequestCommandHandler> _logger;
    public ChangeLeaveRequestApprovalCommandHandler(
         ILeaveRequestRepository leaveRequestRepository,
        IEmailSender emailSender,
        IAppLogger<CancelLeaveRequestCommandHandler> logger
    )
    {
        _leaveRequestRepository = leaveRequestRepository;
        _emailSender = emailSender;
        _logger = logger;
    }
    public async Task<Unit> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
    {
        var leaveRequestToUpdate = await _leaveRequestRepository.GetByIdAsync(request.Id);
        if (leaveRequestToUpdate == null)
        {
            throw new NotFoundException(nameof(Domain.LeaveRequest), request.Id);
        }
        leaveRequestToUpdate.Approved = request.Approved;
        await _leaveRequestRepository.UpdateAsync(leaveRequestToUpdate);
        // Send confirmation message
        try
        {
            var confirmationEmail = new EmailMessage
            {
                To = string.Empty,
                Subject = "Leave Request Approval Update",
                Body = $"You leave request from {leaveRequestToUpdate.StartDate:D} to {leaveRequestToUpdate.EndDate:D} " +
                  $"has been {(request.Approved ? "approved" : "rejected")}"
            };
            await _emailSender.SendEmail(confirmationEmail);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
        }

        return Unit.Value;
    }
}
