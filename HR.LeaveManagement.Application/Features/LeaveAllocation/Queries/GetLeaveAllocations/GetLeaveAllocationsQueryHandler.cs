using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations
{
    public class GetLeaveAllocationsQueryHandler : IRequestHandler<GetLeaveAllocationsQuery, List<GetLeaveAllocationsDto>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        public GetLeaveAllocationsQueryHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
        }


        public async Task<List<GetLeaveAllocationsDto>> Handle(GetLeaveAllocationsQuery request, CancellationToken cancellationToken)
        {
            var leaveAllocations = await _leaveAllocationRepository.GetAllAsync();
            var data = _mapper.Map<List<GetLeaveAllocationsDto>>(leaveAllocations);
            return data;
        }
    }
}
