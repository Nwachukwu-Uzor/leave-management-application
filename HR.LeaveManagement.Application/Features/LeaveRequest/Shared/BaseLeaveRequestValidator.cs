using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Shared;

public class BaseLeaveRequestValidator : AbstractValidator<BaseLeaveRequest>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    public BaseLeaveRequestValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        _leaveTypeRepository = leaveTypeRepository;
        RuleFor(p => p.EndDate)
            .GreaterThan(p => p.StartDate).WithMessage("{PropertyName} must be after than {ComparisonValue}");
        RuleFor(p => p.StartDate)
            .LessThan(p => p.EndDate).WithMessage("{PropertyName} must be before {ComparisonValue}");
        RuleFor(p => p.LeaveTypeId)
            .NotNull().NotEmpty()
            .MustAsync(LeaveTypeExists)
            .WithMessage("{Property} does not exist");
    }

    private async Task<bool> LeaveTypeExists(int leaveTypeId, CancellationToken token)
    {
        return await _leaveTypeRepository.DoesEntityExistAsync(leaveTypeId);
    }
}
