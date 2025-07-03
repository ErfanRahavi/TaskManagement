using Microsoft.AspNetCore.SignalR;
using TaskManagement.Application.Features.Tasks.Dtos;

namespace TaskManagement.API.Hubs
{
    public class TaskNotificationHub : Hub
    {
        public async Task SendTaskAddedNotification(string taskName)
        {
            await Clients.All.SendAsync("ReceiveTaskAddedNotification", taskName);
        }

        public async Task SendTaskUpdatedNotification(TaskDto updatedTask)
        {
            await Clients.All.SendAsync("ReceiveTaskUpdatedNotification", updatedTask);
        }

        public async Task SendTaskDeletedNotification(Guid taskId)
        {
            await Clients.All.SendAsync("ReceiveTaskDeletedNotification", taskId);
        }
    }
}
