using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandValidator : AbstractValidator<CreateLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    public CreateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        _leaveTypeRepository = leaveTypeRepository;

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(70).WithMessage("{PropertyName} must not exceed 70 characters.");

        RuleFor(p => p.DefaultDays)
            .LessThan(100).WithMessage("{PropertyName} must not exceed 100")
            .GreaterThan(1).WithMessage("{PropertyName} must be greater than 1");

        RuleFor(q => q)
            .MustAsync(LeaveTypeNameUnique)
            .WithMessage("Leave Type name must be unique");
        
    }

    private async Task<bool> LeaveTypeNameUnique(CreateLeaveTypeCommand command, CancellationToken token)
    {
        return await _leaveTypeRepository.IsLeaveTypeNameUnique(command.Name);
    }
}
