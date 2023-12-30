using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails
{
    public class GetLeaveTypeDetailsQuery : IRequest<LeaveTypeDetailsDto>
    {
        public int Id { get; }
        public GetLeaveTypeDetailsQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
