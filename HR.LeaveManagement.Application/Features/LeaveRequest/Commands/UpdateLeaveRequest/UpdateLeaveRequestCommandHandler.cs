using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;

public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<UpdateLeaveRequestCommandHandler> _logger;
    private readonly IEmailSender _emailSender;
    public UpdateLeaveRequestCommandHandler(
        ILeaveRequestRepository leaveRequestRepository,
        IMapper mapper,
        IAppLogger<UpdateLeaveRequestCommandHandler> logger,
        ILeaveTypeRepository leaveTypeRepository,
        IEmailSender emailSender)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _mapper = mapper;
        _logger = logger;
        _leaveTypeRepository = leaveTypeRepository;
        _emailSender = emailSender;
    }
    public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateLeaveRequestCommandValidator(_leaveTypeRepository, _leaveRequestRepository);
        var validationResult = validator.Validate(request);
        if (validationResult.Errors.Any())
        {
            throw new BadRequestException("Validation failed for update request", validationResult);
        }
        var leaveRequestToUpdate = await _leaveRequestRepository.GetByIdAsync(request.Id);
        if (leaveRequestToUpdate == null)
        {
            _logger.LogWarning(
                "Unable to find domain entity {0} with key {1}", 
                nameof(Domain.LeaveAllocation), 
                request.Id
            );
            throw new NotFoundException(nameof(Domain.LeaveAllocation), request.Id);
        }
        _mapper.Map(request, leaveRequestToUpdate);
        await _leaveRequestRepository.UpdateAsync(leaveRequestToUpdate);
        // Send confirmation message
        try
        {
            var confirmationEmail = new EmailMessage
            {
                To = string.Empty,
                Subject = "Leave Request Update Successfully",
                Body = $"You leave request from {leaveRequestToUpdate.StartDate:D} to {leaveRequestToUpdate.EndDate:D} " +
                  $"was updated successfully"
            };
            await _emailSender.SendEmail(confirmationEmail);
        } catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
        }
      
        return Unit.Value;
    }
}
