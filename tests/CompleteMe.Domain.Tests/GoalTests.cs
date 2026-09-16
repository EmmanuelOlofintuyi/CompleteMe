using CompleteMe.Domain.Entities;
using CompleteMe.Domain.Enums;
using Xunit;

namespace CompleteMe.Domain.Tests;

// Tests the rules and behavior currently represented by the Goal entity.
public class GoalTests
{
    [Fact]
    // A goal is allowed to exist without being assigned to a project.
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
    // A goal should preserve the status and dates supplied by the caller.
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
