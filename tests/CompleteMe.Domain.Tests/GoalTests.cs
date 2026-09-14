using CompleteMe.Domain.Entities;
using CompleteMe.Domain.Enums;
using Xunit;

namespace CompleteMe.Domain.Tests;

public class GoalTests
{
    [Fact]
    public void Goal_ShouldAllowStandaloneGoalWithoutProject()
    {
        var goal = new Goal
        {
            Id = Guid.NewGuid(),
            Name = "Clean room",
            Description = "Weekly reset",
            Status = GoalStatus.Active
        };

        Assert.Equal("Clean room", goal.Name);
        Assert.Null(goal.ProjectId);
        Assert.Equal(GoalStatus.Active, goal.Status);
    }

    [Fact]
    public void Goal_ShouldKeepStatusAndDateValues()
    {
        var startDate = new DateTime(2026, 9, 13);
        var dueDate = new DateTime(2026, 9, 15);

        var goal = new Goal
        {
            Id = Guid.NewGuid(),
            Name = "Workout",
            StartDate = startDate,
            DueDate = dueDate,
            Status = GoalStatus.Active
        };

        Assert.Equal(startDate, goal.StartDate);
        Assert.Equal(dueDate, goal.DueDate);
        Assert.Equal(GoalStatus.Active, goal.Status);
    }
}
