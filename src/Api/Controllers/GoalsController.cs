using CompleteMe.Application.Services;
using CompleteMe.Application.DTOs.Goals;
using Microsoft.AspNetCore.Mvc;

namespace CompleteMe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoalsController : ControllerBase
{
    private readonly GoalService _goalService;

    public GoalsController(GoalService goalService)
    {
        _goalService = goalService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GoalResponse>>> GetAll()
    {
        var goals = await _goalService.GetAllAsync();
        return Ok(goals);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GoalResponse>> GetById([FromRoute] Guid id)
    {
        var goal = await _goalService.GetByIdAsync(id);
        if (goal == null)
        {
            return NotFound();
        }
        return Ok(goal);
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<List<GoalResponse>>> GetByProjectId([FromRoute] Guid projectId)
    {
        var goals = await _goalService.GetByProjectIdAsync(projectId);
        return Ok(goals);
    }

    [HttpPost]
    public async Task<ActionResult<GoalResponse>> Create(
        [FromBody] CreateGoalRequest request)
    {
        var goal = await _goalService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = goal.Id }, goal);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GoalResponse>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateGoalRequest request)
    {
        var goal = await _goalService.UpdateAsync(id, request);

        if (goal is null)
        {
            return NotFound();
        }

        return Ok(goal);
    }

    [HttpPost("{id}/complete")]
    public async Task<ActionResult<GoalResponse>> Complete(
        [FromRoute] Guid id)
    {
        try
        {
            var goal = await _goalService.CompleteAsync(id);

            if (goal is null)
            {
                return NotFound();
            }

            return Ok(goal);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        await _goalService.DeleteAsync(id);
        return NoContent();
    }
}