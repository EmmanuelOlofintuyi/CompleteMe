using CompleteMe.Application.Services;
using CompleteMe.Domain.Entities;
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
    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await _taskService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetByIdAsync(Guid id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpGet("goal/{goalId}")]
    public async Task<ActionResult<List<TaskItem>>> GetByGoalIdAsync(Guid goalId)
    {
        var tasks = await _taskService.GetByGoalIdAsync(goalId);
        return Ok(tasks);
    }

    [HttpGet("parent/{parentTaskId}")]
    public async Task<ActionResult<List<TaskItem>>> GetByParentTaskIdAsync(Guid parentTaskId)
    {
        var tasks = await _taskService.GetByParentTaskIdAsync(parentTaskId);
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] TaskItem taskItem)
    {
        await _taskService.CreateAsync(taskItem);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = taskItem.Id }, taskItem);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] TaskItem taskItem)
    {
        if (id != taskItem.Id)
        {
            return BadRequest();
        }

        await _taskService.UpdateAsync(taskItem);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _taskService.DeleteAsync(id);
        return NoContent();
    }

}