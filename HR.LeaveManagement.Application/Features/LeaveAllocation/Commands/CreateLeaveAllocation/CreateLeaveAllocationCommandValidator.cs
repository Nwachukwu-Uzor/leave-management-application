using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;

public class CreateLeaveAllocationCommandValidator : AbstractValidator<CreateLeaveAllocationCommand>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    public CreateLeaveAllocationCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        RuleFor(p => p.LeaveTypeId)
            .NotEmpty().WithMessage("Leave type id is required")
            .NotNull()
            .MustAsync(LeaveTypeExists)
            .WithMessage("{PropertyName} is not a valid Leave Type Id"); ;

        RuleFor(p => p.Period)
            .NotNull()
            .NotEmpty()
            .GreaterThan(1).WithMessage("{PropertyName} must be greater than 1")
            .LessThan(100).WithMessage("{PropertyName} must be less than 100");

        RuleFor(p => p.NumberOfDays)
            .NotNull()
            .NotEmpty()
            .GreaterThan(1).WithMessage("{PropertyName} must be greater than 1")
            .LessThan(100).WithMessage("{PropertyName} must be less than 100");

        RuleFor(p => p.EmployeeId)
            .NotNull().NotEmpty().WithMessage("{PropertyName} is required")
            .MinimumLength(1);
        _leaveTypeRepository = leaveTypeRepository;
    }

    private async Task<bool> LeaveTypeExists(int id, CancellationToken token)
    {
        return await _leaveTypeRepository.DoesEntityExistAsync(id);
    }
}
