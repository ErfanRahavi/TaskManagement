using MediatR;
using Microsoft.AspNetCore.SignalR;
using TaskManagement.API.Hubs;
using TaskManagement.Application.Features.Tasks.Commands.UpdateTask;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.API.Handlers
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHubContext<TaskNotificationHub> _hubContext;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository, IHubContext<TaskNotificationHub> hubContext)
        {
            _taskRepository = taskRepository;
            _hubContext = hubContext;
        }

        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);
            if (task == null) return false;

            task.Title = request.Title;
            task.Description = request.Description;
            task.DueDate = request.DueDate;

            await _taskRepository.UpdateAsync(task);

            await _hubContext.Clients.All.SendAsync("ReceiveTaskUpdatedNotification", new
            {
                task.Id,
                task.Title,
                task.Description,
                task.DueDate
            });

            return true;
        }
    }
}
