using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TaskManagement.API.Hubs;
using TaskManagement.Application.Features.Tasks.Commands.CreateTask;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.API.Handlers
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<TaskNotificationHub> _hubContext;
        private readonly ILogger<CreateTaskCommandHandler> _logger;

        public CreateTaskCommandHandler(
            ITaskRepository taskRepository,
            IMapper mapper,
            IHubContext<TaskNotificationHub> hubContext,
            ILogger<CreateTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Mapping CreateTaskCommand to TodoTask entity");
            var todoTask = _mapper.Map<TodoTask>(request);

            _logger.LogInformation("Adding new task to repository with title: {Title}", todoTask.Title);
            await _taskRepository.AddAsync(todoTask);

            _logger.LogInformation("Sending real-time notification for task: {Title}", todoTask.Title);
            await _hubContext.Clients.All.SendAsync("ReceiveTaskAddedNotification", todoTask.Title);

            _logger.LogInformation("Task created successfully with ID: {Id}", todoTask.Id);
            return todoTask.Id;
        }
    }
}
