using CompleteMe.Domain.Entities;
using CompleteMe.Domain.Enums;
using Xunit;

namespace CompleteMe.Domain.Tests;

// Tests the basic relationship rules represented by TaskItem.
public class TaskItemTests
{
    [Fact]
    // A task stores the goal it belongs to.
    public void TaskItem_ShouldRequireGoalReference()
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            GoalId = Guid.NewGuid(),
            Name = "Vacuum floor",
            Status = TaskItemStatus.NotStarted
        };

        Assert.NotEqual(Guid.Empty, task.GoalId);
        Assert.Equal(TaskItemStatus.NotStarted, task.Status);
    }

    [Fact]
    // A task can point to a parent task to support nested work.
    public void TaskItem_ShouldSupportParentTaskRelationship()
    {
        var parentTaskId = Guid.NewGuid();

        var childTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            GoalId = Guid.NewGuid(),
            Name = "Pick up clothes",
            Status = TaskItemStatus.NotStarted,
            ParentTaskId = parentTaskId
        };

        Assert.Equal(parentTaskId, childTask.ParentTaskId);
        Assert.Equal(TaskItemStatus.NotStarted, childTask.Status);
    }
}
