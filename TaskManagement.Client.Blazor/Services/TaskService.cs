using System.Net.Http.Json;
using TaskManagement.Application.Features.Tasks.Dtos;
using TaskManagement.Application.Features.Tasks.Commands.CreateTask;
using TaskManagement.Application.Features.Tasks.Commands.UpdateTask;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace TaskManagement.Client.Blazor.Services
{
    public class TaskService
    {
        private readonly AuthorizedHttpClient _authClient;

        public TaskService(AuthorizedHttpClient authClient)
        {
            _authClient = authClient;
        }

        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            var http = await _authClient.GetClientAsync();
            try
            {
                var result = await http.GetFromJsonAsync<List<TaskDto>>("api/Tasks");
                return result ?? new List<TaskDto>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching tasks: {ex.Message}");
                return new List<TaskDto>();
            }
        }

        public async Task<TaskDto?> GetTaskByIdAsync(Guid id)
        {
            var http = await _authClient.GetClientAsync();
            try
            {
                return await http.GetFromJsonAsync<TaskDto>($"api/Tasks/{id}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching task by ID: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateTaskAsync(CreateTaskCommand command)
        {
            var http = await _authClient.GetClientAsync();
            try
            {
                var response = await http.PostAsJsonAsync("api/Tasks", command);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating task: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateTaskAsync(UpdateTaskCommand command)
        {
            var http = await _authClient.GetClientAsync();
            try
            {
                var response = await http.PutAsJsonAsync($"api/Tasks/{command.Id}", command);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error updating task: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            var http = await _authClient.GetClientAsync();
            try
            {
                var response = await http.DeleteAsync($"api/Tasks/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting task: {ex.Message}");
                return false;
            }
        }
    }
}
