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

        [HttpGet]
        [Route(nameof(GetAll))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<LeaveTypeDto>>> GetAll()
        {
            var response = await _meditor.Send(new GetLeaveTypesQuery());
            return Ok(response);
        }

        [HttpGet]
        [Route(nameof(GetLeaveType) + "/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LeaveTypeDetailsDto>> GetLeaveType(int id)
        {
            var response = await _meditor.Send(new GetLeaveTypeDetailsQuery(id));
            return Ok(response);
        }

        [HttpPost]
        [Route(nameof(CreateLeaveType))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <ActionResult> CreateLeaveType(CreateLeaveTypeCommand command)
        {
            var response = await _meditor.Send(command);
            return CreatedAtAction(nameof(GetLeaveType), new { id = response}, null);
        }
        
        [HttpPut]
        [Route(nameof(UpdateLeaveType))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <ActionResult> UpdateLeaveType(UpdateLeaveTypeCommand command)
        {
            await _meditor.Send(command);
            return NoContent();
        }

        [HttpDelete]
        [Route(nameof(DeleteLeaveType) + "/{id}")]
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
