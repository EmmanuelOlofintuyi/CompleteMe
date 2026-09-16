using CompleteMe.Application.Services;
using CompleteMe.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CompleteMe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]


public class ProjectsController : ControllerBase
{
    private readonly ProjectService _projectService;

    public ProjectsController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<List<Project>> GetAll()
    {
        return await _projectService.GetAllAsync();
    }

    [HttpGet("{id}")]

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        // Retrieves one project, or null when no project matches the id.
        return await _projectService.GetByIdAsync(id);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<List<Project>> GetByCategoryIdAsync(Guid categoryId)
    {
        // Retrieves all projects assigned to one category.
        return await _projectService.GetByCategoryIdAsync(categoryId);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] Project project)
    {
        // Creates a new project by passing it to the repository for persistence.
        await _projectService.CreateAsync(project);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] Project project)
    {
        // Updates an existing project through the repository.
        if (id != project.Id)
        {
            return BadRequest();
        }

        await _projectService.UpdateAsync(project);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
    {
        // Deletes a project by id through the repository.
        await _projectService.DeleteAsync(id);
        return NoContent();
    }
}