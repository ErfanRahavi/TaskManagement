using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TaskManagement.Application.Features.Tasks.Commands.CreateTask;
using TaskManagement.Application.Features.Tasks.Commands.DeleteTask;
using TaskManagement.Application.Features.Tasks.Commands.UpdateTask;
using TaskManagement.Application.Features.Tasks.Dtos;
using TaskManagement.Application.Features.Tasks.Queries;
using TaskManagement.Domain.Entities;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<TasksController> _logger;

        public TasksController(IMediator mediator, IMapper mapper, ILogger<TasksController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTask([FromBody] CreateTaskCommand command)
        {
            _logger.LogInformation("Creating a new task with title: {Title}", command.Title);

            var taskId = await _mediator.Send(command);

            _logger.LogInformation("Task created successfully with ID: {TaskId}", taskId);

            return CreatedAtAction(nameof(GetTaskById), new { id = taskId }, taskId);
        }

        [HttpGet]
        public async Task<ActionResult> GetTasks()
        {
            _logger.LogInformation("Received request to get all tasks");

            var tasks = await _mediator.Send(new GetTaskListQuery());

            _logger.LogInformation("Returned {Count} tasks", tasks.Count);

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTask>> GetTaskById(Guid id)
        {
            _logger.LogInformation("Fetching task with ID: {Id}", id);

            var task = await _mediator.Send(new GetTaskByIdQuery { Id = id });

            if (task == null)
            {
                _logger.LogWarning("No task found with ID: {Id}", id);
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskCommand command)
        {
            _logger.LogInformation("Updating task with ID: {Id}", id);

            if (id != command.Id)
            {
                _logger.LogWarning("Task ID mismatch: URL ID = {Id}, Body ID = {CommandId}", id, command.Id);
                return BadRequest("ID mismatch");
            }

            var result = await _mediator.Send(command);

            if (!result)
            {
                _logger.LogWarning("Task not found for update with ID: {Id}", id);
                return NotFound("Task not found");
            }

            _logger.LogInformation("Task with ID: {Id} updated successfully", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            _logger.LogInformation("Deleting task with ID: {Id}", id);

            var command = new DeleteTaskCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
            {
                _logger.LogWarning("Task not found for deletion with ID: {Id}", id);
                return NotFound("Task not found");
            }

            _logger.LogInformation("Task with ID: {Id} deleted successfully", id);
            return NoContent();
        }
    }
}
