using CompleteMe.Application.DTOs.Goals;
using CompleteMe.Application.Services;
using CompleteMe.Domain.Entities;
using CompleteMe.Domain.Enums;
using CompleteMe.Infrastructure.Repositories;
using Xunit;

namespace CompleteMe.Domain.Tests;

public class GoalServiceTests
{
    [Fact]
    public async Task CompleteAsync_WithIncompleteTask_ThrowsInvalidOperationException()
    {
        var goalRepository = new FakeGoalRepository();
        var taskRepository = new FakeTaskItemRepository();
        var service = new GoalService(goalRepository, taskRepository);

        var goal = CreateGoal();
        await goalRepository.AddAsync(goal);

        await taskRepository.AddAsync(new TaskItem
        {
            Id = Guid.NewGuid(),
            GoalId = goal.Id,
            Name = "Incomplete task",
            Status = TaskItemStatus.InProgress
        });

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CompleteAsync(goal.Id));

        Assert.Equal(GoalStatus.Active, goal.Status);
    }

    [Fact]
    public async Task CompleteAsync_WhenAllTasksAreComplete_CompletesGoal()
    {
        var goalRepository = new FakeGoalRepository();
        var taskRepository = new FakeTaskItemRepository();
        var service = new GoalService(goalRepository, taskRepository);

        var goal = CreateGoal();
        await goalRepository.AddAsync(goal);

        await taskRepository.AddAsync(new TaskItem
        {
            Id = Guid.NewGuid(),
            GoalId = goal.Id,
            Name = "Completed task",
            Status = TaskItemStatus.Completed
        });

        var response = await service.CompleteAsync(goal.Id);

        Assert.NotNull(response);
        Assert.Equal(GoalStatus.Completed, response.Status);
        Assert.Equal(GoalStatus.Completed, goal.Status);
    }

    [Fact]
    public async Task CompleteAsync_WithNoTasks_CompletesGoal()
    {
        var goalRepository = new FakeGoalRepository();
        var taskRepository = new FakeTaskItemRepository();
        var service = new GoalService(goalRepository, taskRepository);

        var goal = CreateGoal();
        await goalRepository.AddAsync(goal);

        var response = await service.CompleteAsync(goal.Id);

        Assert.NotNull(response);
        Assert.Equal(GoalStatus.Completed, response.Status);
    }

    [Fact]
    public async Task UpdateAsync_WhenOnlyNameProvided_LeavesOtherFieldsUntouched()
    {
        var goalRepository = new FakeGoalRepository();
        var taskRepository = new FakeTaskItemRepository();
        var service = new GoalService(goalRepository, taskRepository);

        var originalStart = new DateTime(2026, 9, 1, 8, 0, 0, DateTimeKind.Utc);
        var originalDue = new DateTime(2026, 9, 10, 8, 0, 0, DateTimeKind.Utc);

        var goal = new Goal
        {
            Id = Guid.NewGuid(),
            Name = "Original goal",
            Description = "Original description",
            StartDate = originalStart,
            DueDate = originalDue,
            ProjectId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            RecurrenceRuleId = Guid.NewGuid(),
            Status = GoalStatus.Active
        };

        await goalRepository.AddAsync(goal);

        var request = new UpdateGoalRequest
        {
            Name = "Updated goal"
        };

        var response = await service.UpdateAsync(goal.Id, request);

        Assert.NotNull(response);
        Assert.Equal("Updated goal", response.Name);
        Assert.Equal("Original description", response.Description);
        Assert.Equal(originalStart, response.StartDate);
        Assert.Equal(originalDue, response.DueDate);
        Assert.Equal(goal.ProjectId, response.ProjectId);
        Assert.Equal(goal.CategoryId, response.CategoryId);
        Assert.Equal(goal.RecurrenceRuleId, response.RecurrenceRuleId);
    }

    private static Goal CreateGoal()
    {
        return new Goal
        {
            Id = Guid.NewGuid(),
            Name = "Test goal",
            Status = GoalStatus.Active
        };
    }

    private sealed class FakeGoalRepository : IGoalRepository
    {
        public List<Goal> SavedGoals { get; } = new();

        public Task<List<Goal>> GetAllAsync()
        {
            return Task.FromResult(SavedGoals);
        }

        public Task<Goal?> GetByIdAsync(Guid id)
        {
            var goal = SavedGoals.FirstOrDefault(goal => goal.Id == id);
            return Task.FromResult(goal);
        }

        public Task<List<Goal>> GetByProjectIdAsync(Guid projectId)
        {
            var goals = SavedGoals
                .Where(goal => goal.ProjectId == projectId)
                .ToList();

            return Task.FromResult(goals);
        }

        public Task AddAsync(Goal goal)
        {
            SavedGoals.Add(goal);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Goal goal)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var goal = SavedGoals.FirstOrDefault(goal => goal.Id == id);

            if (goal is not null)
            {
                SavedGoals.Remove(goal);
            }

            return Task.CompletedTask;
        }
    }

    private sealed class FakeTaskItemRepository : ITaskItemRepository
    {
        public List<TaskItem> SavedTasks { get; } = new();

        public Task<List<TaskItem>> GetAllAsync()
        {
            return Task.FromResult(SavedTasks);
        }

        public Task<TaskItem?> GetByIdAsync(Guid id)
        {
            var task = SavedTasks.FirstOrDefault(task => task.Id == id);
            return Task.FromResult(task);
        }

        public Task<List<TaskItem>> GetByGoalIdAsync(Guid goalId)
        {
            var tasks = SavedTasks
                .Where(task => task.GoalId == goalId)
                .ToList();

            return Task.FromResult(tasks);
        }

        public Task<List<TaskItem>> GetByParentTaskIdAsync(Guid parentTaskId)
        {
            var tasks = SavedTasks
                .Where(task => task.ParentTaskId == parentTaskId)
                .ToList();

            return Task.FromResult(tasks);
        }

        public Task AddAsync(TaskItem taskItem)
        {
            SavedTasks.Add(taskItem);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TaskItem taskItem)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var task = SavedTasks.FirstOrDefault(task => task.Id == id);

            if (task is not null)
            {
                SavedTasks.Remove(task);
            }

            return Task.CompletedTask;
        }
    }
}
