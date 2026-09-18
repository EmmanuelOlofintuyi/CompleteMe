using CompleteMe.Application.DTO.Tasks;
using CompleteMe.Domain.Entities;
using CompleteMe.Domain.Enums;
using CompleteMe.Infrastructure.Repositories;

namespace CompleteMe.Application.Services;

public class TaskService
{
    private readonly ITaskItemRepository _taskRepository;

    public TaskService(ITaskItemRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public TaskItemStatus Status { get; set; }

    public async Task<List<TaskResponse>> GetAllAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return tasks.Select(ToResponse).ToList();
    }

    public async Task<TaskResponse?> GetByIdAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task is null ? null : ToResponse(task);
    }

    public async Task<List<TaskResponse>> GetByGoalIdAsync(Guid goalId)
    {
        var tasks = await _taskRepository.GetByGoalIdAsync(goalId);
        return tasks.Select(ToResponse).ToList();
    }

    public async Task<List<TaskResponse>> GetByParentTaskIdAsync(Guid parentTaskId)
    {
        var tasks = await _taskRepository.GetByParentTaskIdAsync(parentTaskId);
        return tasks.Select(ToResponse).ToList();
    }

    public async Task<TaskResponse> CreateAsync(CreateTaskRequest request)
    {
        ValidateTaskName(request.Name);

        if (request.ParentTaskId.HasValue)
        {
            var parentTask = await _taskRepository.GetByIdAsync(
                request.ParentTaskId.Value);

            if (parentTask is null)
            {
                throw new ArgumentException(
                    "The parent task does not exist.",
                    nameof(request.ParentTaskId));
            }
        }

        var now = DateTime.UtcNow;

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            GoalId = request.GoalId,
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            ParentTaskId = request.ParentTaskId,
            CategoryId = request.CategoryId,
            RecurrenceRuleId = request.RecurrenceRuleId,
            Status = TaskItemStatus.NotStarted,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        await _taskRepository.AddAsync(task);

        return ToResponse(task);
    }

public async Task<TaskResponse?> UpdateAsync(Guid id, UpdateTaskRequest request)
{
    var task = await _taskRepository.GetByIdAsync(id);

    if (task is null)
    {
        return null;
    }

    if (request.Name is not null)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("A task name is required.", nameof(request.Name));
        }

        task.Name = request.Name;
    }

    if (request.Description is not null)
    {
        task.Description = request.Description;
    }

    if (request.StartDate.HasValue)
    {
        task.StartDate = request.StartDate.Value;
    }

    if (request.DueDate.HasValue)
    {
        task.DueDate = request.DueDate.Value;
    }

    if (request.CategoryId.HasValue)
    {
        task.CategoryId = request.CategoryId.Value;
    }

    if (request.RecurrenceRuleId.HasValue)
    {
        task.RecurrenceRuleId = request.RecurrenceRuleId.Value;
    }

    if (request.Status.HasValue)
    {
        task.Status = request.Status.Value;
    }

    if (request.ParentTaskId.HasValue)
    {
        var newParentId = request.ParentTaskId.Value;

        if (newParentId == id)
        {
            throw new ArgumentException(
                "A task cannot be its own parent.",
                nameof(request.ParentTaskId));
        }

        var parentTask = await _taskRepository.GetByIdAsync(newParentId);

        if (parentTask is null)
        {
            throw new ArgumentException(
                "The parent task does not exist.",
                nameof(request.ParentTaskId));
        }

        var ancestorId = parentTask.ParentTaskId;

        while (ancestorId.HasValue)
        {
            if (ancestorId.Value == id)
            {
                throw new ArgumentException(
                    "A task cannot become its own ancestor.",
                    nameof(request.ParentTaskId));
            }

            var ancestorTask = await _taskRepository.GetByIdAsync(ancestorId.Value);

            if (ancestorTask is null)
            {
                break;
            }

            ancestorId = ancestorTask.ParentTaskId;
        }

        task.ParentTaskId = newParentId;
    }

    task.UpdatedAtUtc = DateTime.UtcNow;

    await _taskRepository.UpdateAsync(task);

    return ToResponse(task);
}

    public Task DeleteAsync(Guid id)
    {
        return _taskRepository.DeleteAsync(id);
    }

    private static void ValidateTaskName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "A task name is required.",
                nameof(name));
        }
    }

    private static TaskResponse ToResponse(TaskItem task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            GoalId = task.GoalId,
            Name = task.Name,
            Description = task.Description,
            StartDate = task.StartDate,
            DueDate = task.DueDate,
            Status = task.Status,
            CreatedAtUtc = task.CreatedAtUtc,
            UpdatedAtUtc = task.UpdatedAtUtc,
            ParentTaskId = task.ParentTaskId,
            CategoryId = task.CategoryId,
            RecurrenceRuleId = task.RecurrenceRuleId
        };
    }
}