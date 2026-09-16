using CompleteMe.Domain.Entities;
using CompleteMe.Infrastructure.Repositories;

namespace CompleteMe.Application.Services;

// Coordinates project-related use cases between the API and the repository.
// Business rules can be added here without placing them in the database layer.
public class ProjectService
{
    // The service depends on the repository contract rather than directly on EF Core.
    private readonly IProjectRepository _projectRepository;

    // The repository is supplied through dependency injection.
    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public Task<List<Project>> GetAllAsync()
    {
        // Retrieves every project, including goals loaded by the repository.
        return _projectRepository.GetAllAsync();
    }

    public Task<Project?> GetByIdAsync(Guid id)
    {
        // Retrieves one project, or null when no project matches the id.
        return _projectRepository.GetByIdAsync(id);
    }

    public Task<List<Project>> GetByCategoryIdAsync(Guid categoryId)
    {
        // Retrieves all projects assigned to one category.
        return _projectRepository.GetByCategoryIdAsync(categoryId);
    }

    public Task CreateAsync(Project project)
    {
        // Creates a new project by passing it to the repository for persistence.
        return _projectRepository.AddAsync(project);
    }

    public Task UpdateAsync(Project project)
    {
        // Updates an existing project through the repository.
        return _projectRepository.UpdateAsync(project);
    }

    public Task DeleteAsync(Guid id)
    {
        // Deletes a project by id through the repository.
        return _projectRepository.DeleteAsync(id);
    }
}