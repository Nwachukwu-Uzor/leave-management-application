using MediatR;
using Microsoft.AspNetCore.Mvc;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType;

namespace HR.LeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly IMediator _meditor;
        public LeaveTypeController(IMediator meditor)
        {
            _meditor = meditor;
        }

        [HttpGet(nameof(GetAll))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<LeaveTypeDto>>> GetAll()
        {
            var response = await _meditor.Send(new GetLeaveTypesQuery());
            return Ok(response);
        }

        [HttpGet($"{nameof(GetLeaveType)}/{{id}}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LeaveTypeDetailsDto>> GetLeaveType(int id)
        {
            var response = await _meditor.Send(new GetLeaveTypeDetailsQuery(id));
            return Ok(response);
        }

        [HttpPost(nameof(CreateLeaveType))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <ActionResult> CreateLeaveType(CreateLeaveTypeCommand command)
        {
            await _meditor.Send(command);
            // return CreatedAtAction(nameof(GetLeaveType), new {id = reponse});
            return NoContent();
        }
        
        [HttpPut(nameof(UpdateLeaveType))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <ActionResult> UpdateLeaveType(UpdateLeaveTypeCommand command)
        {
            await _meditor.Send(command);
            // return CreatedAtAction(nameof(GetLeaveType), new {id = reponse});
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> DeleteLeaveType(int id)
        {
            await _meditor.Send(new DeleteLeaveTypeCommand(id));
            return NoContent();
        }
    }
}
