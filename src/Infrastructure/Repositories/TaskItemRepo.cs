using CompleteMe.Domain.Entities;
using CompleteMe.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CompleteMe.Infrastructure.Repositories;

// Defines the task data operations used by the application layer.
public interface ITaskItemRepository
{
    // Returns every task, including the related parent and child task data.
    Task<List<TaskItem>> GetAllAsync();

    // Returns one task by id, or null when no matching task exists.
    Task<TaskItem?> GetByIdAsync(Guid id);

    // Returns all tasks that belong to a specific goal.
    Task<List<TaskItem>> GetByGoalIdAsync(Guid goalId);

    // Returns the direct child tasks under a specific parent task.
    Task<List<TaskItem>> GetByParentTaskIdAsync(Guid ParentTaskId);

    // Adds a new task to the database.
    Task AddAsync(TaskItem taskItem);

    // Saves changes to an existing task.
    Task UpdateAsync(TaskItem taskItem);

    // Removes a task by its id.
    Task DeleteAsync(Guid id);
}

// Implements task data access using EF Core.
public class TaskItemRepo : ITaskItemRepository
{
    // The context provides access to the TaskItems table.
    private readonly AppDbContext _context;

    public TaskItemRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        // Load parent and child tasks so callers receive the task hierarchy.
        return await _context.TaskItems
            .Include(t => t.ParentTask)
            .Include(t => t.ChildTasks)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        // Return one task with its immediate hierarchy, or null if it is missing.
        return await _context.TaskItems
            .Include(t => t.ParentTask)
            .Include(t => t.ChildTasks)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<TaskItem>> GetByGoalIdAsync(Guid goalId)
    {
        // Find all tasks belonging to one goal.
        return await _context.TaskItems
            .Where(t => t.GoalId == goalId)
            .ToListAsync();
    }

    public async Task<List<TaskItem>> GetByParentTaskIdAsync(Guid parentTaskId)
    {
        // Find the direct children of one parent task.
        return await _context.TaskItems
            .Where(t => t.ParentTaskId == parentTaskId)
            .ToListAsync();
    }

    public async Task AddAsync(TaskItem taskItem)
    {
        // Mark the task for insertion, then commit it to the database.
        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem taskItem)
    {
        // Mark the task's current values for update, then commit them.
        _context.TaskItems.Update(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        // Find the tracked entity before asking EF Core to remove it.
        var taskItem = await _context.TaskItems.FindAsync(id);
        if (taskItem is null)
            return;

        _context.TaskItems.Remove(taskItem);
        // SaveChangesAsync sends the deletion to the database.
        await _context.SaveChangesAsync();
    }
}