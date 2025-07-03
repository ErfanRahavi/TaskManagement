using MediatR;
using Microsoft.AspNetCore.SignalR;
using TaskManagement.API.Hubs;
using TaskManagement.Application.Features.Tasks.Commands.DeleteTask;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.API.Handlers
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHubContext<TaskNotificationHub> _hubContext;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository, IHubContext<TaskNotificationHub> hubContext)
        {
            _taskRepository = taskRepository;
            _hubContext = hubContext;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);
            if (task == null) return false;

            await _taskRepository.DeleteAsync(request.Id);

            
            await _hubContext.Clients.All.SendAsync("ReceiveTaskDeletedNotification", request.Id);

            return true;
        }
    }
}
