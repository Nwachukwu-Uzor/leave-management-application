using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaveAllocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveAllocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(GetAll))]
    public async Task<ActionResult<List<GetLeaveAllocationsDto>>> GetAll()
    {
        var response = await _mediator.Send(new GetLeaveAllocationsQuery());
        return Ok(response);
    }

    [HttpGet($"{nameof(GetById)}/{{id}}")]
    public async Task<ActionResult<GetLeaveAllocationsDetailsDto>> GetById(int id)
    {
        var leaveAllocation = await _mediator.Send(new GetLeaveAllocationDetailsQuery(id));
        return Ok(leaveAllocation);
    }

    [HttpPost(nameof(CreateLeaveAllocation))]
    public async Task<ActionResult> CreateLeaveAllocation(CreateLeaveAllocationCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut(nameof(UpdateLeaveAllocation))]
    public async Task<ActionResult> UpdateLeaveAllocation(UpdateLeaveAllocationCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete($"{nameof(DeleteLeaveAllocation)}/{{id}}")]
    public async Task<ActionResult> DeleteLeaveAllocation(int id)
    {
        await _mediator.Send(new DeleteLeaveAllocationCommand(id));
        return NoContent();
    }
}
