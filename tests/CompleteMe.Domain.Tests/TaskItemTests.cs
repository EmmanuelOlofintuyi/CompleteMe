using CompleteMe.Domain.Entities;
using CompleteMe.Domain.Enums;
using Xunit;

namespace CompleteMe.Domain.Tests;

public class TaskItemTests
{
    [Fact]
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
