using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestsList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LeaveRequestsListDto>> GetAll()
        {
            var response = await _mediator.Send(new GetLeaveRequestsListQuery());
            return Ok(response);
        }

        [HttpGet]
        [Route(nameof(GetById) + "/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LeaveRequestDetailsDto>> GetById(int id)
        {
            var response = await _mediator.Send(new GetLeaveRequestDetailsQuery(id));
            return Ok(response);
        }

        [HttpPost(nameof(CreateLeaveRequest))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateLeaveRequest(CreateLeaveRequestCommand command)
        {
            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = response}, null);
        }

        [HttpPut]
        [Route(nameof(UpdateLeaveRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateLeaveRequest(UpdateLeaveRequestCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut]
        [Route(nameof(CancelLeaveRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CancelLeaveRequest(CancelLeaveRequestCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut]
        [Route(nameof(ChangeLeaveRequestApproval))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangeLeaveRequestApproval(ChangeLeaveRequestApprovalCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete]
        [Route(nameof(DeleteLeaveRequest) + "/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteLeaveRequest(int id)
        {
            await _mediator.Send(new DeleteLeaveRequestCommand(id));
            return NoContent();
        }
    }
}
