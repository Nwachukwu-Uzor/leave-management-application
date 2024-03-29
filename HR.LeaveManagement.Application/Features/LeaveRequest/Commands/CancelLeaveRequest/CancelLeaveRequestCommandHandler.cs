﻿using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;

public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, Unit>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IEmailSender _emailSender;
    private readonly IAppLogger<CancelLeaveRequestCommandHandler> _logger;
    public CancelLeaveRequestCommandHandler(
        ILeaveRequestRepository leaveRequestRepository, 
        IEmailSender emailSender, 
        IAppLogger<CancelLeaveRequestCommandHandler> logger
    )
    {
        _leaveRequestRepository = leaveRequestRepository;
        _emailSender = emailSender;
        _logger = logger;
    }
    public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequestToCancel = await _leaveRequestRepository.GetByIdAsync(request.Id);
        if (leaveRequestToCancel == null)
        {
            throw new NotFoundException(nameof(Domain.LeaveRequest), request.Id);
        }
        leaveRequestToCancel.Cancelled = true;
        await _leaveRequestRepository.UpdateAsync(leaveRequestToCancel);
        // Send confirmation message
        try
        {
            var confirmationEmail = new EmailMessage
            {
                To = string.Empty,
                Subject = "Leave Request Cancelled",
                Body = $"You leave request from {leaveRequestToCancel.StartDate:D} to {leaveRequestToCancel.EndDate:D} " +
                  $"has been cancelled"
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
