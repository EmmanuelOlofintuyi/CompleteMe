namespace CompleteMe.Domain.Enums;

// Lifecycle states that a goal can move through.
public enum GoalStatus
{
    Active,
    Paused,
    Completed,
    Archived,
    Dismissed
}