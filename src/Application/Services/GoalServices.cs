using CompleteMe.Application.DTOs.Goals;
using CompleteMe.Domain.Entities;
using CompleteMe.Domain.Enums;
using CompleteMe.Infrastructure.Repositories;

namespace CompleteMe.Application.Services;

public class GoalService
{
    private readonly IGoalRepository _goalRepository;

    public GoalService(IGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
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

    public async Task<GoalResponse?> UpdateAsync(
        Guid id,
        UpdateGoalRequest request)
    {
        var goal = await _goalRepository.GetByIdAsync(id);

        if (goal is null)
        {
            return null;
        }

        goal.Name = request.Name;
        goal.Description = request.Description;
        goal.StartDate = request.StartDate;
        goal.DueDate = request.DueDate;
        goal.ProjectId = request.ProjectId;
        goal.CategoryId = request.CategoryId;
        goal.RecurrenceRuleId = request.RecurrenceRuleId;
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
}