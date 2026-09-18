using CompleteMe.Application.DTO.Tasks;
using CompleteMe.Application.Services;
using CompleteMe.Domain.Entities;
using CompleteMe.Infrastructure.Repositories;
using Xunit;
using CompleteMe.Domain.Enums;

namespace CompleteMe.Domain.Tests;

public class TaskServiceTests
{
    [Fact]
    public async Task CreateAsync_WithParentTaskId_PreservesParentRelationship()
    {
        var repository = new FakeTaskItemRepository();
        var service = new TaskService(repository);

        var goalId = Guid.NewGuid();
        var parentTaskId = Guid.NewGuid();

        await repository.AddAsync(new TaskItem
        {
            Id = parentTaskId,
            GoalId = goalId,
            Name = "Read chapter two",
            Status = TaskItemStatus.NotStarted
        });

        var request = new CreateTaskRequest
        {
            GoalId = goalId,
            ParentTaskId = parentTaskId,
            Name = "Write chapter notes",
            Description = "Summarize the important ideas"
        };

        var response = await service.CreateAsync(request);

        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(goalId, response.GoalId);
        Assert.Equal(parentTaskId, response.ParentTaskId);
        Assert.Equal("Write chapter notes", response.Name);
        Assert.NotEqual(default, response.CreatedAtUtc);
        Assert.NotEqual(default, response.UpdatedAtUtc);

        var savedTask = repository.SavedTasks
            .Single(task => task.Id == response.Id);

        Assert.Equal(response.Id, savedTask.Id);
        Assert.Equal(parentTaskId, savedTask.ParentTaskId);
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
            var existingTask = SavedTasks
                .First(task => task.Id == taskItem.Id);

            SavedTasks.Remove(existingTask);
            SavedTasks.Add(taskItem);

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

    [Fact]
    public async Task CreateAsync_WithoutParentTaskId_CreatesTopLevelTask()
    {
        var repository = new FakeTaskItemRepository();
        var service = new TaskService(repository);

        var response = await service.CreateAsync(new CreateTaskRequest
        {
            GoalId = Guid.NewGuid(),
            Name = "Read chapter two"
        });

        Assert.Null(response.ParentTaskId);
        Assert.Equal(TaskItemStatus.NotStarted, response.Status);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesTaskProperties()
    {
    var repository = new FakeTaskItemRepository();
    var service = new TaskService(repository);

    var task = new TaskItem
    {
        Id = Guid.NewGuid(),
        GoalId = Guid.NewGuid(),
        Name = "Initial task",
        Description = "Initial description",
        DueDate = DateTime.UtcNow.AddDays(1),
        Status = TaskItemStatus.NotStarted
    };

    await repository.AddAsync(task);

    var request = new UpdateTaskRequest
    {
        Name = "Updated task",
        Description = "Updated description",
        DueDate = DateTime.UtcNow.AddDays(2)
    };

    var response = await service.UpdateAsync(task.Id, request);

    Assert.NotNull(response);
    Assert.Equal(task.Id, response.Id);
    Assert.Equal("Updated task", response.Name);
    Assert.Equal("Updated description", response.Description);
    Assert.Equal(request.DueDate, response.DueDate);
    Assert.NotEqual(default, response.UpdatedAtUtc);

    var savedTask = Assert.Single(repository.SavedTasks);

    Assert.Equal("Updated task", savedTask.Name);
    Assert.Equal("Updated description", savedTask.Description);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentTask_ReturnsNull()
    {
        var repository = new FakeTaskItemRepository();
        var service = new TaskService(repository);

        var request = new UpdateTaskRequest
        {
            Name = "Non-existent task",
            Description = "This task does not exist"
        };

        var response = await service.UpdateAsync(Guid.NewGuid(), request);

        Assert.Null(response);
    }

    [Fact]
    public async Task DeleteAsync_RemovesExistingTask()
    {
        var repository = new FakeTaskItemRepository();
        var service = new TaskService(repository);

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            GoalId = Guid.NewGuid(),
            Name = "Task to delete",
            Status = TaskItemStatus.NotStarted
        };

        await repository.AddAsync(task);

        await service.DeleteAsync(task.Id);

        Assert.Empty(repository.SavedTasks);
    }

    [Fact]
    public async Task UpdateAsync_WhenParentTaskIsSelf_ThrowsArgumentException()
    {
        var repository = new FakeTaskItemRepository();
        var service = new TaskService(repository);

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            GoalId = Guid.NewGuid(),
            Name = "Existing task",
            Status = TaskItemStatus.NotStarted
        };

        await repository.AddAsync(task);

        var request = new UpdateTaskRequest
        {
            Name = "Updated task",
            ParentTaskId = task.Id
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateAsync(task.Id, request));
    }

    [Fact]
    public async Task UpdateAsync_WhenParentIsDescendant_ThrowsArgumentException()
    {
        var repository = new FakeTaskItemRepository();
        var service = new TaskService(repository);

        var goalId = Guid.NewGuid();

        var rootTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            GoalId = goalId,
            Name = "Root task",
            Status = TaskItemStatus.NotStarted
        };

        var childTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            GoalId = goalId,
            Name = "Child task",
            ParentTaskId = rootTask.Id,
            Status = TaskItemStatus.NotStarted
        };

        var grandchildTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            GoalId = goalId,
            Name = "Grandchild task",
            ParentTaskId = childTask.Id,
            Status = TaskItemStatus.NotStarted
        };

        await repository.AddAsync(rootTask);
        await repository.AddAsync(childTask);
        await repository.AddAsync(grandchildTask);

        var request = new UpdateTaskRequest
        {
            Name = "Updated root task",
            ParentTaskId = grandchildTask.Id
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateAsync(rootTask.Id, request));
    }

    [Fact]
    public async Task CreateAsync_WithMissingParentTask_ThrowsArgumentException()
    {
        var repository = new FakeTaskItemRepository();
        var service = new TaskService(repository);

        var request = new CreateTaskRequest
        {
            GoalId = Guid.NewGuid(),
            ParentTaskId = Guid.NewGuid(),
            Name = "Orphan child task"
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(request));

        Assert.Empty(repository.SavedTasks);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyName_ThrowsArgumentException()
    {
        var repository = new FakeTaskItemRepository();
        var service = new TaskService(repository);

        var request = new CreateTaskRequest
        {
            GoalId = Guid.NewGuid(),
            Name = "   "
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(request));

        Assert.Empty(repository.SavedTasks);
    }

    [Fact]
    public async Task UpdateAsync_WithEmptyName_ThrowsArgumentException()
    {
        var repository = new FakeTaskItemRepository();
        var service = new TaskService(repository);

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            GoalId = Guid.NewGuid(),
            Name = "Existing task",
            Status = TaskItemStatus.NotStarted
        };

        await repository.AddAsync(task);

        var request = new UpdateTaskRequest
        {
            Name = string.Empty
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateAsync(task.Id, request));

        Assert.Equal("Existing task", repository.SavedTasks[0].Name);
    }
}
