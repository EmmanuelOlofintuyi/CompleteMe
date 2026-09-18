using CompleteMe.Application.Services;
using CompleteMe.Application.DTO.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CompleteMe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TaskController : ControllerBase
{
    private readonly TaskService _taskService;

    public TaskController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskResponse>>> GetAllAsync()
    {
        return await _taskService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponse>> GetById(Guid id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpGet("goal/{goalId}")]
    public async Task<ActionResult<List<TaskResponse>>> GetByGoalIdAsync(Guid goalId)
    {
        var tasks = await _taskService.GetByGoalIdAsync(goalId);
        return Ok(tasks);
    }

    [HttpGet("parent/{parentTaskId}")]
    public async Task<ActionResult<List<TaskResponse>>> GetByParentTaskIdAsync(Guid parentTaskId)
    {
        var tasks = await _taskService.GetByParentTaskIdAsync(parentTaskId);
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> CreateAsync(
        [FromBody] CreateTaskRequest request)
    {
        var task = await _taskService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskResponse>> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateTaskRequest request)
    {
        var task = await _taskService.UpdateAsync(id, request);

        if (task is null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _taskService.DeleteAsync(id);
        return NoContent();
    }

}