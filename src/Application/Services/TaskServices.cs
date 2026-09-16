using CompleteMe.Domain.Entities;
using CompleteMe.Infrastructure.Repositories;

namespace CompleteMe.Application.Services;

// Coordinates task-related use cases between the API and the repository.
// Business rules can be added here without putting them in the database layer.
public class TaskService
{
    // The service depends on the repository contract, not directly on EF Core.
    private readonly ITaskItemRepository _taskRepository;

    // The repository is supplied through dependency injection.
    public TaskService(ITaskItemRepository taskRepository)
	{
		_taskRepository = taskRepository;
	} 

    // Retrieves all tasks and their loaded task hierarchy.
    public Task<List<TaskItem>> GetAllAsync()
    {
        return _taskRepository.GetAllAsync();
    }

    // Retrieves one task by id, or null when no task matches the id.
    public Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return _taskRepository.GetByIdAsync(id);
    }

    // Retrieves all tasks belonging to one goal.
    public Task<List<TaskItem>> GetByGoalIdAsync(Guid goalId)
    {
        return _taskRepository.GetByGoalIdAsync(goalId);
    }

    // Retrieves the direct child tasks belonging to one parent task.
    public Task<List<TaskItem>> GetByParentTaskIdAsync(Guid parentTaskId)
    {
        return _taskRepository.GetByParentTaskIdAsync(parentTaskId);
    }

    // Creates a new task by passing it to the repository for persistence.
    public  Task CreateAsync(TaskItem taskItem)
    {
        return _taskRepository.AddAsync(taskItem);
    }

    // Updates an existing task through the repository.
    public Task UpdateAsync(TaskItem taskItem)
    {
        return _taskRepository.UpdateAsync(taskItem);
    }

    // Deletes a task by id through the repository.
    public Task DeleteAsync(Guid id)
    {
        return _taskRepository.DeleteAsync(id);
    }
}