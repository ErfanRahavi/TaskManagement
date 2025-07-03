using Microsoft.AspNetCore.SignalR.Client;
using TaskManagement.Application.Features.Tasks.Dtos;

namespace TaskManagement.Client.Blazor.Services
{
    public class SignalRService
    {
        private HubConnection? _hubConnection;

        public event Action<string>? OnTaskAdded;
        public event Action<TaskDto>? OnTaskUpdated;  
        public event Action<Guid>? OnTaskDeleted;

        public async Task StartAsync()
        {
            if (_hubConnection == null)
            {
                _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7171/taskNotificationHub")
                .WithAutomaticReconnect()
                .AddJsonProtocol(options => {
                    options.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
                })
                .Build();

                _hubConnection.On<string>("ReceiveTaskAddedNotification", taskName =>
                {
                    OnTaskAdded?.Invoke(taskName);
                });

                _hubConnection.On<TaskDto>("ReceiveTaskUpdatedNotification", updatedTask =>
                {
                    OnTaskUpdated?.Invoke(updatedTask);
                });

                _hubConnection.On<Guid>("ReceiveTaskDeletedNotification", taskId =>
                {
                    OnTaskDeleted?.Invoke(taskId);
                });
            }

            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync();
            }
        }

        public async Task StopAsync()
        {
            if (_hubConnection is { State: not HubConnectionState.Disconnected })
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
            }
        }
    }
}
