using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Queries
{
    public class GetTaskListQueryHandler : IRequestHandler<GetTaskListQuery, List<TodoTask>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskListQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<TodoTask>> Handle(GetTaskListQuery request, CancellationToken cancellationToken)
        {
            return (await _taskRepository.GetAllAsync()).ToList();
        }
    }
}
