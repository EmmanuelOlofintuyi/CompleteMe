using CompleteMe.Application.DTOs.Goals;
using CompleteMe.Domain.Entities;
using CompleteMe.Domain.Enums;
using CompleteMe.Infrastructure.Repositories;

namespace CompleteMe.Application.Services;

public class GoalService
{
    private readonly IGoalRepository _goalRepository;
    private readonly ITaskItemRepository _taskRepository;

    public GoalService(
        IGoalRepository goalRepository,
        ITaskItemRepository taskRepository)
    {
        _goalRepository = goalRepository;
        _taskRepository = taskRepository;
    }

    public async Task<List<GoalResponse>> GetAllAsync()
    {
        var goals = await _goalRepository.GetAllAsync();

        return goals.Select(ToResponse).ToList();
    }

    public async Task<GoalResponse?> GetByIdAsync(Guid id)
    {
        var goal = await _goalRepository.GetByIdAsync(id);

        return goal is null ? null : ToResponse(goal);
    }

    public async Task<List<GoalResponse>> GetByProjectIdAsync(Guid projectId)
    {
        var goals = await _goalRepository.GetByProjectIdAsync(projectId);

        return goals.Select(ToResponse).ToList();
    }

    public async Task<GoalResponse> CreateAsync(CreateGoalRequest request)
    {
        var now = DateTime.UtcNow;

        var goal = new Goal
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId,
            CategoryId = request.CategoryId,
            RecurrenceRuleId = request.RecurrenceRuleId,
            Status = GoalStatus.Active,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        await _goalRepository.AddAsync(goal);

        return ToResponse(goal);
    }

public async Task<GoalResponse?> UpdateAsync(Guid id, UpdateGoalRequest request)
{
    var goal = await _goalRepository.GetByIdAsync(id);

    if (goal is null)
    {
        return null;
    }

    if (request.Name is not null)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("A goal name is required.", nameof(request.Name));
        }

        goal.Name = request.Name;
    }

    if (request.Description is not null)
    {
        goal.Description = request.Description;
    }

    if (request.StartDate.HasValue)
    {
        goal.StartDate = request.StartDate.Value;
    }

    if (request.DueDate.HasValue)
    {
        goal.DueDate = request.DueDate.Value;
    }

    if (request.ProjectId.HasValue)
    {
        goal.ProjectId = request.ProjectId.Value;
    }

    if (request.CategoryId.HasValue)
    {
        goal.CategoryId = request.CategoryId.Value;
    }

    if (request.RecurrenceRuleId.HasValue)
    {
        goal.RecurrenceRuleId = request.RecurrenceRuleId.Value;
    }

    goal.UpdatedAtUtc = DateTime.UtcNow;

    await _goalRepository.UpdateAsync(goal);

    return ToResponse(goal);
}

    public Task DeleteAsync(Guid id)
    {
        return _goalRepository.DeleteAsync(id);
    }

    private static GoalResponse ToResponse(Goal goal)
    {
        return new GoalResponse
        {
            Id = goal.Id,
            Name = goal.Name,
            Description = goal.Description,
            StartDate = goal.StartDate,
            DueDate = goal.DueDate,
            ProjectId = goal.ProjectId,
            Status = goal.Status,
            CreatedAtUtc = goal.CreatedAtUtc,
            UpdatedAtUtc = goal.UpdatedAtUtc,
            CategoryId = goal.CategoryId,
            RecurrenceRuleId = goal.RecurrenceRuleId
        };
    }

    public async Task<GoalResponse?> CompleteAsync(Guid id)
    {
        var goal = await _goalRepository.GetByIdAsync(id);

        if (goal is null)
        {
            return null;
        }

        var tasks = await _taskRepository.GetByGoalIdAsync(id);

        var hasIncompleteTasks = tasks.Any(task =>
            task.Status != TaskItemStatus.Completed);

        if (hasIncompleteTasks)
        {
            throw new InvalidOperationException(
                "A goal cannot be completed while it has incomplete tasks.");
        }

        goal.Status = GoalStatus.Completed;
        goal.UpdatedAtUtc = DateTime.UtcNow;

        await _goalRepository.UpdateAsync(goal);

        return ToResponse(goal);
    }
}