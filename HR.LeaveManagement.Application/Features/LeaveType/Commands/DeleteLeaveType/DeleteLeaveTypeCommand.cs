using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommand : IRequest<Unit>
    {
        public int Id { get; }
        public DeleteLeaveTypeCommand(int Id)
        {
            this.Id = Id;
        }
    }
}
